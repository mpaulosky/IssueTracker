# Architecture Overview

Issue Tracker follows Clean Architecture principles to ensure separation of concerns, maintainability, and testability.

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                     Presentation Layer                  │
│                  (IssueTracker.UI)                      │
│              Blazor Components & Pages                   │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                   Application Layer                      │
│                 (IssueTracker.Services)                 │
│            Business Logic & Use Cases                    │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                     Domain Layer                         │
│               (IssueTracker.CoreBusiness)               │
│           Entities, Interfaces, Models                   │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                 Infrastructure Layer                     │
│                (IssueTracker.PlugIns)                   │
│             Data Access & External Services              │
└─────────────────────────────────────────────────────────┘
```

## Layer Responsibilities

### 1. Presentation Layer (`IssueTracker.UI`)

**Purpose**: User interface and user interaction

**Contains**:
- Blazor components
- Razor pages
- UI state management
- Client-side validation

**Dependencies**:
- Services layer for business operations
- CoreBusiness for models

**Key Principles**:
- No direct database access
- Minimal business logic (UI-specific only)
- Dependency injection for services

### 2. Application Layer (`IssueTracker.Services`)

**Purpose**: Application business logic and orchestration

**Contains**:
- Service implementations
- Use case orchestration
- Application-level validation
- DTOs and mapping logic

**Dependencies**:
- CoreBusiness for models and interfaces
- PlugIns for data access (via dependency injection)

**Key Principles**:
- Implements business rules
- Coordinates between UI and data access
- Technology-agnostic

### 3. Domain Layer (`IssueTracker.CoreBusiness`)

**Purpose**: Core business models

**Contains**:
- Domain entities (Issue, User, Comment, Category, Status)
- Business validation rules
- Enums and value objects
- Database settings interfaces

**Dependencies**:
- None (pure domain logic)

**Key Principles**:
- Framework-independent
- No infrastructure concerns
- Rich domain models

### 4. Infrastructure Layer (`IssueTracker.PlugIns`)

**Purpose**: External concerns and data persistence

**Contains**:
- MongoDB repository implementations
- Database context
- External service integrations
- Infrastructure-specific code

**Dependencies**:
- CoreBusiness for interfaces and models

**Key Principles**:
- Implements data access interfaces
- Technology-specific implementations
- Easily swappable (e.g., switch from MongoDB to SQL)

## Data Flow

### Example: Creating a New Issue

```
User Input (UI)
    │
    ▼
Blazor Component validates input
    │
    ▼
Calls IssueService.CreateIssueAsync()
    │
    ▼
Service validates business rules
    │
    ▼
Calls IIssueRepository.CreateAsync()
    │
    ▼
MongoDb Repository saves to database
    │
    ▼
Returns result up the chain
    │
    ▼
UI updates and displays confirmation
```

## Key Design Patterns

### 1. Repository Pattern

Abstracts data access behind interfaces:

```csharp
// In CoreBusiness (interface)
public interface IIssueRepository
{
    Task<Issue> CreateAsync(Issue issue);
    Task<Issue?> GetByIdAsync(string id);
    Task<List<Issue>> GetAllAsync();
    // ...
}

// In PlugIns (implementation)
public class IssueRepository : IIssueRepository
{
    private readonly IMongoCollection<Issue> _collection;
    // Implementation details...
}
```

### 2. Dependency Injection

All dependencies are injected:

```csharp
public class IssueService
{
    private readonly IIssueRepository _repository;
    
    public IssueService(IIssueRepository repository)
    {
        _repository = repository;
    }
}
```

### 3. Service Layer Pattern

Business logic is encapsulated in services:

```csharp
public class IssueService : IIssueService
{
    public async Task<Issue> CreateIssueAsync(Issue issue)
    {
        // Validate
        // Apply business rules
        // Persist
        // Return result
    }
}
```

## Technology Stack

### Frontend
- **Blazor Server**: Interactive UI with server-side rendering
- **Bootstrap CSS**: Responsive design
- **Razor Components**: Reusable UI components

### Backend
- **.NET 7**: Modern framework
- **C#**: Primary language
- **ASP.NET Core**: Web framework

### Data Access
- **MongoDB.Driver**: Official MongoDB client
- **MongoDB**: Document database

### Testing
- **xUnit**: Test framework
- **bUnit**: Blazor component testing
- **Bogus**: Test data generation
- **Testcontainers**: Docker-based integration tests

## Project Organization

```
src/
├── CoreBusiness/
│   └── IssueTracker.CoreBusiness/
│       ├── Models/              # Domain entities
│       ├── Contracts/           # Repository interfaces
│       ├── Enum/               # Enumerations
│       └── Helpers/            # Domain utilities
│
├── PlugIns/
│   ├── IssueTracker.PlugIns/
│   │   ├── DataAccess/         # MongoDB repositories
│   │   └── Contracts/          # Infrastructure interfaces
│   │
│   └── IssueTracker.PlugIns.Mongo/
│       └── ...                 # Future: additional providers
│
├── Services/
│   └── IssueTracker.Services/
│       ├── Issue/              # Issue-related services
│       ├── User/               # User management
│       ├── Comment/            # Comment services
│       ├── Category/           # Category services
│       └── Status/             # Status services
│
└── UI/
    └── IssueTracker.UI/
        ├── Components/         # Reusable components
        ├── Pages/             # Routable pages
        └── Shared/            # Shared layouts
```

## Benefits of This Architecture

### 1. **Testability**
- Each layer can be tested independently
- Repository interfaces enable easy mocking
- Business logic isolated from infrastructure
- Comprehensive test coverage across unit and integration tests

### 2. **Maintainability**
- Clear separation of concerns
- Changes isolated to specific layers
- Easy to understand and navigate
- Well-organized service structure by domain entity

### 3. **Flexibility**
- Easy to swap MongoDB for other database providers
- Repository pattern abstracts data access
- Support for multiple UI frameworks
- MongoDB-specific features in separate PlugIns project

### 4. **Scalability**
- Service-oriented architecture
- Clear boundaries for future microservices migration
- Easy to add new entities and services
- Supports horizontal scaling

## Dependency Rules

The dependency flow is strictly enforced:

```
UI → Services → CoreBusiness
      ↓
    PlugIns → CoreBusiness
```

**Core Rule**: Dependencies point inward (toward the domain)

- UI depends on Services and CoreBusiness
- Services depend on CoreBusiness and define repository interfaces
- PlugIns depend on CoreBusiness (implement repository interfaces)
- CoreBusiness depends on nothing

## Related Documentation

- [Project Structure](project-structure.md)
- [Design Patterns](design-patterns.md)
- [Database Schema](database-schema.md)
- [Testing Guide](testing.md)
