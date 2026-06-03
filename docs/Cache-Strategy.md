## Cache Strategy & Implementation

### Overview

Issue Tracker implements a three-tier caching strategy using **Redis** as the distributed cache
backend. Caching improves application performance by storing frequently accessed data, reducing
database load and latency.

### What Is Cached and Why

Caching is applied to operations where:

- Data is **read-heavy** (more reads than writes)
- Response time matters for user experience
- Data does not require strict real-time consistency
- Stale data is acceptable within a defined window

**Data NOT cached**:

- Authentication tokens (use session storage)
- User passwords or sensitive credentials
- Frequently-changing metrics (real-time dashboards)
- Transient error states

### Three-Tier Caching Strategy

#### Tier 1: Query Results (5-Minute TTL)

Stores the results of expensive database queries.

**Use Case**: Listing all issues, fetching user profiles

**TTL**: 5 minutes (`TimeSpan.FromMinutes(5)`)

**Example**:

```csharp
var cacheKey = "issues:all";
var issues = await _cacheService.GetAsync<List<Issue>>(cacheKey);

if (issues is null)
{
  // Cache miss: fetch from database
  issues = await _issueRepository.GetAllAsync();
  
  // Store in cache for 5 minutes
  await _cacheService.SetAsync(cacheKey, issues, TimeSpan.FromMinutes(5));
}

return issues;
```

**Invalidation**: When issues are created, updated, or deleted

#### Tier 2: Output (10-Minute TTL)

Stores rendered or processed output, such as formatted reports or aggregated data.

**Use Case**: Issue statistics, dashboard summaries

**TTL**: 10 minutes (`TimeSpan.FromMinutes(10)`)

**Example**:

```csharp
var cacheKey = "report:issue-summary";
var report = await _cacheService.GetAsync<IssueReport>(cacheKey);

if (report is null)
{
  // Cache miss: generate report
  report = await _reportService.GenerateSummaryAsync();
  
  // Store in cache for 10 minutes
  await _cacheService.SetAsync(cacheKey, report, TimeSpan.FromMinutes(10));
}

return report;
```

**Invalidation**: When underlying data changes or on schedule

#### Tier 3: Session (1-Hour TTL)

Stores user-specific state and preferences.

**Use Case**: User settings, recent searches, filter state

**TTL**: 1 hour (`TimeSpan.FromHours(1)`)

**Example**:

```csharp
var userId = currentUser.Id;
var cacheKey = $"session:user:{userId}:preferences";
var prefs = await _cacheService.GetAsync<UserPreferences>(cacheKey);

if (prefs is null)
{
  prefs = new UserPreferences { /* defaults */ };
  await _cacheService.SetAsync(cacheKey, prefs, TimeSpan.FromHours(1));
}

return prefs;
```

**Invalidation**: When user updates preferences or session expires

### Using ICacheService

#### Injection

Register `ICacheService` in your service class via constructor injection:

```csharp
public class IssueService
{
  private readonly ICacheService _cacheService;
  private readonly IIssueRepository _repository;
  private readonly ILogger<IssueService> _logger;

  public IssueService(
    ICacheService cacheService,
    IIssueRepository repository,
    ILogger<IssueService> logger)
  {
    _cacheService = cacheService;
    _repository = repository;
    _logger = logger;
  }
}
```

#### Core Operations

**Get from Cache**:

```csharp
public async Task<Issue?> GetIssueByIdAsync(string id)
{
  var cacheKey = $"issue:{id}";
  var issue = await _cacheService.GetAsync<Issue>(cacheKey);

  if (issue is not null)
  {
    _logger.LogDebug("Cache hit for issue {IssueId}", id);
    return issue;
  }

  // Fetch from database and cache
  issue = await _repository.GetByIdAsync(id);
  if (issue is not null)
  {
    await _cacheService.SetAsync(cacheKey, issue, TimeSpan.FromMinutes(5));
  }

  return issue;
}
```

**Set in Cache**:

```csharp
await _cacheService.SetAsync(
  key: "mydata:key",
  value: myObject,
  expiration: TimeSpan.FromMinutes(5)
);
```

**Remove from Cache**:

```csharp
await _cacheService.RemoveAsync("issue:123");
```

### Cache Key Naming Convention

Use hierarchical, dot-separated keys for clarity and organization.

**Format**: `{domain}:{entity}:{id}:{variant}`

**Examples**:

```
issues:all                           # All issues (Tier 1)
issues:all:active                    # Active issues only
issues:list:page:1                   # Paginated list, page 1
issue:123                            # Single issue by ID
issue:123:comments                   # Issue with comments
issue:123:activity:full              # Full activity audit
report:issue-summary                 # Issue summary report
session:user:john-doe:preferences    # User preferences
session:user:john-doe:recent-search  # Recent searches
```

**Benefits**:

- Easy to find related cache entries
- Clear pattern for debugging
- Simplifies bulk invalidation (use prefix matching)

### Cache Invalidation Patterns

#### Pattern 1: Immediate Invalidation (On Write)

Invalidate cache immediately when data changes.

```csharp
public async Task UpdateIssueAsync(string id, UpdateIssueRequest request)
{
  // Update in database
  var issue = await _repository.UpdateAsync(id, request);

  // Invalidate related caches
  await _cacheService.RemoveAsync($"issue:{id}");
  await _cacheService.RemoveAsync("issues:all");

  return issue;
}
```

**Pros**: Data consistency, no stale cache

**Cons**: Cache may become empty frequently, reducing hit rates

#### Pattern 2: Time-Based Expiration (TTL)

Let cache expire naturally after TTL.

```csharp
// Cache expires after 5 minutes automatically
await _cacheService.SetAsync(
  "issues:all",
  issues,
  TimeSpan.FromMinutes(5)
);
```

**Pros**: Simple, less code

**Cons**: Stale data for up to TTL duration

#### Pattern 3: Lazy Invalidation

Combine both: invalidate on critical updates, let others expire naturally.

```csharp
public async Task DeleteIssueAsync(string id)
{
  // Critical operation: invalidate immediately
  await _cacheService.RemoveAsync($"issue:{id}");
  await _cacheService.RemoveAsync("issues:all");

  // Delete from database
  await _repository.DeleteAsync(id);
}

public async Task GetIssuesAsync()
{
  // Non-critical: rely on TTL
  var cacheKey = "issues:all";
  var issues = await _cacheService.GetAsync<List<Issue>>(cacheKey);
  
  if (issues is null)
  {
    issues = await _repository.GetAllAsync();
    await _cacheService.SetAsync(cacheKey, issues, TimeSpan.FromMinutes(5));
  }

  return issues;
}
```

**Pros**: Balanced performance and consistency

**Cons**: Requires careful key management

#### Pattern 4: Event-Driven Invalidation

Publish events when data changes; subscribe to invalidate caches.

```csharp
// When an issue is updated
public class IssueUpdatedEvent
{
  public string IssueId { get; set; }
}

// Subscriber
public class CacheInvalidationHandler : INotificationHandler<IssueUpdatedEvent>
{
  private readonly ICacheService _cache;

  public async Task Handle(IssueUpdatedEvent notification, CancellationToken ct)
  {
    await _cache.RemoveAsync($"issue:{notification.IssueId}");
    await _cache.RemoveAsync("issues:all");
  }
}
```

**Pros**: Decoupled, scales well with many cache keys

**Cons**: Requires event infrastructure (MediatR, etc.)

### When NOT to Cache

**Security Data**:

- Passwords, API keys, tokens (store in session only)

**Frequently-Changing Data**:

- Real-time metrics, stock prices, user counts

**Large Objects**:

- Videos, files (use CDN or object storage instead)

**User-Specific Data**:

- Sensitive information that must not leak between users (be careful with key scoping)

**Data with Strict Consistency Requirements**:

- Financial transactions, critical operations

### Monitoring Cache Performance

Monitor cache hit/miss rates to optimize TTLs:

```csharp
logger.LogInformation(
  "Cache operation: {CacheKey}, Status: {Status}",
  cacheKey,
  hitOrMiss
);
```

Check logs for patterns:

- High hit rate on Tier 1 (Query Results) = good strategy
- Low hit rate = TTL too short or keys not reused
- Stale data complaints = TTL too long

### Cache Serialization

`ICacheService` uses `System.Text.Json` for serialization. Ensure cached objects are JSON-serializable:

- Use `[JsonPropertyName]` for property mapping if needed
- Avoid circular references
- Use standard .NET types (List, Dictionary, etc.)

**Example**:

```csharp
public class Issue
{
  [JsonPropertyName("id")]
  public string Id { get; set; }

  [JsonPropertyName("title")]
  public string Title { get; set; }
}

// This will cache/deserialize correctly
await _cache.SetAsync("issue:1", issue, TimeSpan.FromMinutes(5));
```

### Error Handling

`ICacheService` logs serialization errors and removes corrupted entries:

```csharp
try
{
  var result = await _cache.GetAsync<Issue>(key);
}
catch (JsonException ex)
{
  logger.LogWarning(ex, "Failed to deserialize cached value");
  // Entry is automatically removed; next request fetches fresh data
}
```

Your code does not need explicit error handling for cache operations.
