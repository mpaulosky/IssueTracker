# Project Structure

This document provides a detailed overview of the Issue Tracker project organization and folder structure.

## Solution Structure

```
IssueTracker/
├── src/                    # Source code
├── tests/                  # Test projects
├── docs/                   # Documentation
├── TestResults/           # Test coverage reports
└── [Configuration Files]  # Solution-level configs
```

## Source Code (`src/`)

### Core Business (`src/CoreBusiness/`)

The heart of the domain logic, containing entities and business rules.

```
CoreBusiness/
└── IssueTracker.CoreBusiness/
    ├── Models/                    # Domain entities
    │   ├── BasicIssueModel.cs
    │   ├── BasicUserModel.cs
    │   ├── BasicCommentModel.cs
    │   ├── BasicCategoryModel.cs
    │   ├── BasicStatusModel.cs
    │   ├── IssueModel.cs
    │   ├── UserModel.cs
    │   ├── CommentModel.cs
    │   ├── CategoryModel.cs
    │   ├── StatusModel.cs
    │   └── DatabaseSettings.cs
    │
    ├── Contracts/                 # Infrastructure interfaces
    │   └── IDatabaseSettings.cs
    │
    ├── Enum/                      # Enumerations
    │   └── [Domain enumerations]
    │
    ├── Helpers/                   # Domain utilities
    │   └── [Validation and helper methods]
    │
    ├── BogusFakes/               # Test data generators
    │   └── [Faker implementations]
    │
    └── GlobalUsings.cs           # Global namespace imports
```

**Key Points**:
- Pure C# - no framework dependencies
- Contains no implementation details
- Domain models with validation
- Provides data contracts for Services and PlugIns

### Plugins (`src/PlugIns/`)

Infrastructure layer containing data access and external integrations.

```
PlugIns/
├── IssueTracker.PlugIns/
│   ├── DataAccess/               # MongoDB implementations
│   │   ├── IssueRepository.cs
│   │   ├── UserRepository.cs
│   │   ├── CommentRepository.cs
│   │   ├── CategoryRepository.cs
│   │   ├── StatusRepository.cs
│   │   └── MongoDbContext.cs
│   │
│   ├── Contracts/                # Infrastructure interfaces
│   │   └── IDbContext.cs
│   │
│   └── GlobalUsings.cs
│
└── IssueTracker.PlugIns.Mongo/   # Future: MongoDB-specific features
    └── [Additional MongoDB implementations]
```

**Key Points**:
- Implements repository interfaces from CoreBusiness
- MongoDB-specific code
- Easily swappable for other databases
- No business logic

### Services (`src/Services/`)

Application services that orchestrate business logic and define repository interfaces.

```
Services/
└── IssueTracker.Services/
    ├── Issue/                         # Issue-related services
    │   ├── Interface/
    │   │   └── IIssueService.cs
    │   └── IssueService.cs
    │
    ├── User/                          # User management
    │   └── UserService.cs
    │
    ├── Comment/                       # Comment services
    │   └── CommentService.cs
    │
    ├── Category/                      # Category services
    │   └── CategoryService.cs
    │
    ├── Status/                        # Status services
    │   └── StatusService.cs
    │
    └── PlugInRepositoryInterfaces/   # Repository interfaces
        ├── IIssueRepository.cs
        ├── IUserRepository.cs
        ├── ICommentRepository.cs
        ├── ICategoryRepository.cs
        └── IStatusRepository.cs
```

**Key Points**:
- Implements business use cases
- Coordinates between UI and data access
- Validates business rules
- Defines repository contracts (interfaces) for data access
- Services depend on repository interfaces abstraction

### UI (`src/UI/`)

Blazor Server-based user interface with component-driven architecture.

```
UI/
└── IssueTracker.UI/
    ├── Components/               # Reusable Razor components
    │   ├── CommentComponent.razor
    │   ├── CommentCreateComponent.razor
    │   ├── IssueComponent.razor
    │   ├── SetStatusComponent.razor
    │   └── MyInputRadioGroup.cs
    │
    ├── Pages/                    # Routable Razor pages
    │   ├── Index.razor           # Home page
    │   ├── Admin.razor           # Admin panel
    │   ├── Categories.razor      # Category management
    │   ├── Statuses.razor        # Status management
    │   ├── Create.razor          # Create issue
    │   ├── Details.razor         # Issue details
    │   ├── Comment.razor         # Comments page
    │   ├── Profile.razor         # User profile
    │   └── SampleData.razor      # Sample data
    │
    ├── Shared/                   # Shared layouts
    │   └── MainLayout.razor      # Main layout
    │
    ├── Extensions/               # DI configuration
    │   ├── AllServicesToRegister.cs
    │   ├── AuthenticationService.cs
    │   ├── AuthorizationService.cs
    │   ├── RegisterConnections.cs
    │   ├── RegisterDatabaseContext.cs
    │   ├── RegisterPlugInRepositories.cs
    │   └── RegisterServicesCollections.cs
    │
    ├── Helpers/                  # Helper classes
    │   └── [UI helper utilities]
    │
    ├── Models/                   # UI-specific DTOs
    │   ├── CreateIssueDto.cs
    │   └── CreateCommentDto.cs
    │
    ├── wwwroot/                  # Static files
    │   ├── css/
    │   ├── js/
    │   └── images/
    │
    ├── App.razor                 # Root component
    ├── Program.cs                # Application entry point
    ├── appsettings.json          # Configuration
    ├── appsettings.Development.json
    ├── _Imports.razor            # Global Razor imports
    └── Dockerfile                # Docker configuration
```

**Key Points**:
- Blazor Server components with code-behind pattern
- Component-based architecture for reusability
- Dependency injection for services
- Authentication and authorization integrated
- Minimal business logic (UI-specific only)
- Bootstrap CSS for responsive styling
- Docker support for containerization

## Tests (`tests/`)

Comprehensive test suite covering all layers.

```
tests/
├── IssueTracker.CoreBusiness.Tests.Unit/
│   ├── Models/                   # Entity tests
│   ├── Helpers/                  # Helper tests
│   └── BogusFakes/              # Faker tests
│
├── IssueTracker.PlugIns.Tests.Unit/
│   └── DataAccess/              # Repository unit tests
│
├── IssueTracker.PlugIns.Tests.Integration/
│   ├── DataAccess/              # MongoDB integration tests
│   ├── DatabaseCollection.cs    # Test collection config
│   └── IssueTrackerTestFactory.cs
│
├── IssueTracker.Services.Tests.Unit/
│   ├── Issue/                   # Issue service tests
│   ├── User/                    # User service tests
│   ├── Comment/                 # Comment service tests
│   ├── Category/                # Category service tests
│   └── Status/                  # Status service tests
│
├── IssueTracker.UI.Tests.Unit/
│   ├── Components/              # Component tests
│   ├── Pages/                   # Page tests
│   └── Helpers/                 # UI helper tests
│
└── TestingSupport.Library/
    └── Fixtures/                # Shared test fixtures
```

**Key Points**:
- Separate unit and integration tests
- xUnit test framework
- bUnit for Blazor testing
- Testcontainers for integration tests

## Documentation (`docs/`)

Comprehensive project documentation.

```
docs/
├── index.md                     # Documentation home
├── getting-started.md           # Setup guide
├── architecture.md              # Architecture overview
├── project-structure.md         # This file
├── testing.md                   # Testing guide
├── CONTRIBUTING.md              # Contribution guidelines
├── CODE_OF_CONDUCT.md          # Community standards
├── CODE_METRICS.md             # Code quality metrics
├── SECURITY.md                 # Security policy
└── REFERENCES.md               # API references
```

## Configuration Files

### Root Level

```
IssueTracker/
├── IssueTracker.slnx           # Solution file
├── Directory.Packages.props    # Central package management
├── Global.json                 # .NET SDK version
├── docker-compose.yml          # Docker orchestration
├── dotnet.config              # NuGet configuration
├── codecov.yml                # Code coverage config
├── dependabot.yml             # Dependency updates
├── runsettings.xml            # Test settings
├── testEnvironments.json      # Test environment config
├── IssueTracker.lutconfig     # LUT configuration
└── LICENSE                     # MIT License
```

### Project-Level

Each project contains:

```
[Project]/
├── [Project].csproj            # Project file
├── GlobalUsings.cs             # Global using directives
├── bin/                        # Build output (not in source control)
└── obj/                        # Build intermediates (not in source control)
```

## Dependency Flow

```
    IssueTracker.UI
         │
         ├─→ IssueTracker.Services
         │         │
         │         └─→ IssueTracker.CoreBusiness
         │                      ▲
         └─────────────────────┼─────────┐
                                │         │
                    IssueTracker.PlugIns  │
                                          │
                              (implements interfaces)
```

**Rules**:
- UI depends on Services and CoreBusiness
- Services depend on CoreBusiness
- PlugIns depend on CoreBusiness (implements interfaces)
- CoreBusiness has no dependencies

## Build Output

```
TestResults/
├── [guid]/                     # Test run results
│   └── coverage.opencover.xml  # Coverage data
└── ...
```

## Key Design Decisions

### 1. Clean Architecture

Projects are organized by architectural layer, not by feature. This enforces:
- Clear separation of concerns
- Testability
- Maintainability

### 2. Vertical Slice by Entity

Within Services and tests, code is organized by entity/domain concept:
- Issue
- User
- Comment
- Category
- Status

This makes it easy to find and modify related code.

### 3. Global Usings

Each project has a `GlobalUsings.cs` file to reduce repetitive using statements:

```csharp
// Example: GlobalUsings.cs
global using System;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using Microsoft.Extensions.DependencyInjection;
```

### 4. Central Package Management

`Directory.Packages.props` centralizes NuGet package versions:

```xml
<PackageVersion Include="MongoDB.Driver" Version="2.19.0" />
<PackageVersion Include="xunit" Version="2.4.2" />
```

All projects reference the same versions, ensuring consistency.

## Navigation Tips

### Finding Code

1. **Domain Models**: `src/CoreBusiness/IssueTracker.CoreBusiness/Models/`
2. **Data Access**: `src/PlugIns/IssueTracker.PlugIns/DataAccess/`
3. **Business Logic**: `src/Services/IssueTracker.Services/[Entity]/`
4. **UI Components**: `src/UI/IssueTracker.UI/Components/`
5. **Tests**: `tests/[Layer].Tests.Unit/` or `.Tests.Integration/`

### Common Tasks

- **Add new entity**: Start in `CoreBusiness/Models/`, then add repository interface, implementation, and service
- **Add new page**: Create `.razor` file in `UI/Pages/`
- **Add new component**: Create `.razor` file in `UI/Components/`
- **Add tests**: Create test class in appropriate test project

## Related Documentation

- [Architecture Overview](architecture.md)
- [Getting Started](getting-started.md)
- [Testing Guide](testing.md)
- [Contributing](CONTRIBUTING.md)
