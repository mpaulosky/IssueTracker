# History — Wolinski

## Project Learnings (from init)

**Project:** IssueTracker  
**API Framework:** ASP.NET Core Minimal APIs, .NET 10  
**Database:** MongoDB (MongoDB.Driver + MongoDB.EntityFrameworkCore)  
**Orchestration:** .NET Aspire (AppHost, ServiceDefaults)  
**Owner:** mpaulosky  
**Goal:** Aspireify and enhance the application  

**API Patterns:**
- Minimal APIs in `Program.cs` or map extensions
- OpenAPI via Scalar (not Swagger UI)
- Request DTOs with FluentValidation
- Async/await: `async Task<IResult> GetIssue(int id, IIssueService service) => ...`
- File-scoped namespaces, PascalCase types

**Database (MongoDB):**
- Repositories: `IIssueRepository`, `ICommentRepository`, etc.
- DbContext pooling if using EF Core MongoDB
- Change tracking for updates
- Async operations: `await repo.GetIssueAsync(id)`

**Aspire Architecture:**
- AppHost defines services and resources
- ServiceDefaults provides shared middleware (logging, health checks, tracing)
- Each service registers: `.AddServiceDefaults()` in Program.cs
- Resources: containers (MongoDB), API endpoints

**Testing:**
- Integration tests with TestContainers (Docker MongoDB)
- xUnit + FluentAssertions
- Test both happy path and error cases

## Learnings

(None yet — first session)
