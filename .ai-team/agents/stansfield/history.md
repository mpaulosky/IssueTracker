# History — Stansfield

## Project Learnings (from init)

**Project:** IssueTracker  
**UI Framework:** Blazor Server, .NET 10  
**Styling:** Tailwind CSS (no hardcoded colors; use scoped .razor.css)  
**Owner:** mpaulosky  
**Goal:** Aspireify and enhance the application  

**Component Patterns:**
- `.razor` files with `@page` (for pages) or `@inherits ComponentBase` (for reusable components)
- Suffix: `Component` for reusable, `Page` for routable
- File-scoped namespaces, PascalCase types
- Parameters via `[Parameter]`, event callbacks via `[Parameter] public EventCallback<T> OnEvent`
- Lifecycle: `OnInitializedAsync` (data), `OnParametersSetAsync` (param changes)
- Inject services: `@inject IIssueService IssueService`
- Render fragments: `[Parameter] public RenderFragment? ChildContent { get; set; }`

**Testing:**
- bUnit tests in `tests/Web.Tests.Bunit/`
- Test component renders, parameter changes, user interactions
- Snapshot tests for layouts

**Styling:**
- Tailwind utility classes in `.razor` markup
- Component-scoped styles in `.razor.css`
- Mobile-first responsive design

## Learnings

### AppHost Integration (2026-02-16)

**Solution Structure:**
- AppHost is the orchestrator entry point; goes in `/src/AppHost/` alongside other source projects
- `.slnx` file needs explicit folder structure `<Folder Name="/src/AppHost/">` before project reference
- Build order is implicit in Aspire graph; no need for manual dependency ordering in slnx

**Aspire & Blazor UI:**
- Blazor UI (IssueTracker.UI) is referenced as project dependency in AppHost Program.cs, not as separate web host
- Aspire handles port binding, health checks, and service lifecycle — UI doesn't need appsettings.json for MongoDB connection
- Health checks critical for Aspire readiness; dashboard (`:15000`) is primary tool for observing service state

**Developer UX Patterns:**
- Single `dotnet run --project AppHost` is game-changing for onboarding — eliminates "mystery" steps
- Troubleshooting docs must cover both Aspire orchestration AND underlying service issues (Docker, ports, timeouts)
- Expected startup time (10-15s) should be documented to prevent false failure assumptions
- Aspire dashboard is essential for developers to understand what's running and why

**Documentation:**
- Getting-started.md should emphasize (1) one command, (2) what that command does, (3) where to look for debugging
- Obsolete patterns (docker-compose, manual appsettings) must be removed to prevent confusion
- Prerequisites should reflect actual deps (Docker + .NET SDK); optional tools should be noted separately
