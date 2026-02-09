# Testing Guide

Issue Tracker has comprehensive test coverage across multiple layers, including unit tests and integration tests.

## Test Structure

```
tests/
├── IssueTracker.CoreBusiness.Tests.Unit/      # Domain & business logic tests
├── IssueTracker.PlugIns.Tests.Unit/           # Data access unit tests
├── IssueTracker.PlugIns.Tests.Integration/    # Integration tests with MongoDB
├── IssueTracker.Services.Tests.Unit/          # Service layer tests
├── IssueTracker.UI.Tests.Unit/                # UI component tests
└── TestingSupport.Library/                    # Shared test utilities
```

## Running Tests

### Run All Tests

```bash
dotnet test
```

### Run Specific Test Project

```bash
# Core business logic tests
dotnet test tests/IssueTracker.CoreBusiness.Tests.Unit

# Service layer tests
dotnet test tests/IssueTracker.Services.Tests.Unit

# Integration tests (requires Docker)
dotnet test tests/IssueTracker.PlugIns.Tests.Integration

# UI component tests
dotnet test tests/IssueTracker.UI.Tests.Unit
```

### Run Tests with Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Run Tests in Watch Mode

```bash
dotnet watch test --project tests/IssueTracker.CoreBusiness.Tests.Unit
```

## Test Types

### 1. Unit Tests

**Purpose**: Test individual components in isolation

**Characteristics**:
- Fast execution
- No external dependencies
- Use mocking/fakes
- Test single units of code

**Example**: Testing a service method

```csharp
public class IssueServiceTests
{
    [Fact]
    public async Task CreateIssueAsync_ValidIssue_ReturnsCreatedIssue()
    {
        // Arrange
        var mockRepo = new Mock<IIssueRepository>();
        var service = new IssueService(mockRepo.Object);
        var issue = new Issue { Title = "Test Issue" };
        
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<Issue>()))
                .ReturnsAsync(issue);
        
        // Act
        var result = await service.CreateIssueAsync(issue);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Issue", result.Title);
    }
}
```

### 2. Integration Tests

**Purpose**: Test interactions with real external systems

**Characteristics**:
- Test database operations
- Use Testcontainers for MongoDB
- Slower execution
- Test multiple components together

**Example**: Testing MongoDB repository

```csharp
[Collection("Database")]
public class IssueRepositoryTests : IAsyncLifetime
{
    private readonly IssueTrackerTestFactory _factory;
    private IIssueRepository _repository;
    
    public IssueRepositoryTests(IssueTrackerTestFactory factory)
    {
        _factory = factory;
    }
    
    public async Task InitializeAsync()
    {
        _repository = _factory.GetRepository<IIssueRepository>();
    }
    
    [Fact]
    public async Task CreateAsync_ValidIssue_SavesSuccessfully()
    {
        // Arrange
        var issue = new Issue { Title = "Integration Test" };
        
        // Act
        var result = await _repository.CreateAsync(issue);
        
        // Assert
        Assert.NotNull(result.Id);
        
        var retrieved = await _repository.GetByIdAsync(result.Id);
        Assert.Equal("Integration Test", retrieved.Title);
    }
    
    public Task DisposeAsync() => Task.CompletedTask;
}
```

### 3. UI Component Tests (bUnit)

**Purpose**: Test Blazor components

**Characteristics**:
- Test component rendering
- Test user interactions
- Simulate events
- Verify component state

**Example**: Testing a Blazor component

```csharp
public class IssueListComponentTests : TestContext
{
    [Fact]
    public void IssueList_RendersCorrectly()
    {
        // Arrange
        var issues = new List<Issue>
        {
            new Issue { Id = "1", Title = "Issue 1" },
            new Issue { Id = "2", Title = "Issue 2" }
        };
        
        Services.AddSingleton<IIssueService>(
            Mock.Of<IIssueService>(s => s.GetAllAsync() == Task.FromResult(issues))
        );
        
        // Act
        var cut = RenderComponent<IssueListComponent>();
        
        // Assert
        cut.MarkupMatches("<div>...</div>"); // Match expected markup
        Assert.Contains("Issue 1", cut.Markup);
        Assert.Contains("Issue 2", cut.Markup);
    }
}
```

## Testing Technologies

### Core Testing Tools

- **xUnit**: Primary test framework
- **FluentAssertions**: Readable assertions
- **Moq**: Mocking framework
- **Bogus**: Test data generation
- **AutoFixture**: Automated test data
- **bUnit**: Blazor component testing
- **Testcontainers**: Docker-based MongoDB integration tests
- **Moq**: Mocking framework for unit tests

### Test Data Generation

Using **Bogus** for realistic test data:

```csharp
public class IssueGenerator
{
    private readonly Faker<Issue> _faker;
    
    public IssueGenerator()
    {
        _faker = new Faker<Issue>()
            .RuleFor(i => i.Title, f => f.Lorem.Sentence())
            .RuleFor(i => i.Description, f => f.Lorem.Paragraph())
            .RuleFor(i => i.CreatedDate, f => f.Date.Recent())
            .RuleFor(i => i.Status, f => f.PickRandom<IssueStatus>());
    }
    
    public Issue Generate() => _faker.Generate();
    
    public List<Issue> Generate(int count) => _faker.Generate(count);
}
```

## Integration Test Setup

### Docker-based MongoDB

Integration tests use Testcontainers to spin up real MongoDB instances:

```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private MongoDbContainer _mongoContainer;
    
    public string ConnectionString { get; private set; }
    
    public async Task InitializeAsync()
    {
        _mongoContainer = new MongoDbBuilder()
            .WithImage("mongo:latest")
            .Build();
            
        await _mongoContainer.StartAsync();
        
        ConnectionString = _mongoContainer.GetConnectionString();
    }
    
    public async Task DisposeAsync()
    {
        await _mongoContainer.DisposeAsync();
    }
}
```

### Test Collections

Using xUnit collections to share database instances:

```csharp
[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<IssueTrackerTestFactory>
{
    // This class has no code, and is never created.
    // Its purpose is simply to be the place to apply [CollectionDefinition]
}
```

## Code Coverage

### Viewing Coverage Reports

After running tests with coverage:

```bash
# Generate coverage report
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# View in browser (requires reportgenerator)
reportgenerator -reports:coverage.opencover.xml -targetdir:coverage-report
```

### Coverage Goals

- **Unit Tests**: Aim for >80% code coverage
- **Integration Tests**: Cover critical paths
- **UI Tests**: Cover main user workflows

### Current Coverage

Check the [Code Metrics](CODE_METRICS.md) for detailed coverage statistics.

## Best Practices

### 1. Arrange-Act-Assert (AAA) Pattern

```csharp
[Fact]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange: Set up test data and dependencies
    var mockRepo = new Mock<IRepository>();
    var service = new Service(mockRepo.Object);
    
    // Act: Execute the method being tested
    var result = await service.DoSomething();
    
    // Assert: Verify the expected outcome
    Assert.NotNull(result);
}
```

### 2. Test Naming Convention

```
MethodName_Scenario_ExpectedBehavior
```

Examples:
- `CreateIssue_ValidData_ReturnsCreatedIssue`
- `DeleteIssue_NonExistentId_ThrowsNotFoundException`
- `UpdateIssue_NullInput_ThrowsArgumentNullException`

### 3. One Assert Per Test (Guideline)

Focus each test on a single behavior:

```csharp
// Good
[Fact]
public void Add_TwoNumbers_ReturnsSum()
{
    Assert.Equal(5, Calculator.Add(2, 3));
}

// Less ideal (testing multiple things)
[Fact]
public void Calculator_VariousOperations_WorkCorrectly()
{
    Assert.Equal(5, Calculator.Add(2, 3));
    Assert.Equal(6, Calculator.Multiply(2, 3));
    Assert.Equal(1, Calculator.Subtract(3, 2));
}
```

### 4. Use Theory for Parameterized Tests

```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(0, 0, 0)]
[InlineData(-1, 1, 0)]
public void Add_VariousInputs_ReturnsCorrectSum(int a, int b, int expected)
{
    Assert.Equal(expected, Calculator.Add(a, b));
}
```

### 5. Isolate External Dependencies

```csharp
// Use mocks for external dependencies
var mockEmailService = new Mock<IEmailService>();
var mockLogger = new Mock<ILogger<IssueService>>();

var service = new IssueService(
    mockRepository.Object,
    mockEmailService.Object,
    mockLogger.Object
);
```

## CI/CD Integration

Tests run automatically on:

- Every push to any branch
- Every pull request
- Scheduled nightly builds

### GitHub Actions Workflow

```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal /p:CollectCoverage=true

- name: Upload Coverage
  uses: codecov/codecov-action@v3
  with:
    files: coverage.opencover.xml
```

## Troubleshooting

### Integration Tests Failing

**Issue**: Integration tests can't connect to MongoDB

**Solutions**:
1. Ensure Docker is running
2. Check Docker Desktop has enough resources
3. Verify Testcontainers can pull images
4. Check firewall settings

### Slow Test Execution

**Issue**: Tests take too long to run

**Solutions**:
1. Run unit tests separately: `dotnet test --filter Category!=Integration`
2. Use parallel execution (enabled by default in xUnit)
3. Run specific test projects instead of all tests

### Coverage Not Generated

**Issue**: Coverage reports not being created

**Solutions**:
1. Install coverlet: `dotnet tool install -g coverlet.console`
2. Ensure test projects reference `coverlet.collector`
3. Check output directory for coverage files

## Related Documentation

- [Architecture Overview](architecture.md)
- [Project Structure](project-structure.md)
- [Code Metrics](CODE_METRICS.md)
- [Contributing Guide](CONTRIBUTING.md)
