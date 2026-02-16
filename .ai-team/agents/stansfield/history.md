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

(None yet — first session)
