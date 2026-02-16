# Routing

Routing rules determine which agent handles which type of work.

## Primary Routing

| Domain | Agent | Examples |
|--------|-------|----------|
| **Architecture, decisions, code review, scope** | Milo (Lead) | "Set up Aspire", design reviews, approve PRs, triage issues |
| **Blazor UI, components, client state, styling** | Stansfield (Frontend) | Build components, refactor views, Blazor state management |
| **APIs, MongoDB, services, business logic, Aspire config** | Wolinski (Backend) | Implement endpoints, add repositories, Aspire orchestration |
| **Tests, quality, coverage, edge cases, test automation** | Hooper (Tester) | Write xUnit/bUnit tests, validate requirements, quality gates |
| **Session logging, decision merging, context management** | Scribe | Always — logs and syncs team state (never spawned on task) |
| **Work queue monitoring, issue triage, backlog management** | Ralph (if active) | Scans GitHub, keeps team moving, prevents idle time |

## Decision Gates

| Gate | Owner | Trigger | Action |
|------|-------|---------|--------|
| Architecture | Milo | Multi-system changes, Aspire design | Approve or reject before agents proceed |
| Code Review | Milo | All PRs before merge | Approve, request changes, or reject |
| Test Coverage | Hooper | Features without tests | Reject until coverage acceptable |

## Ceremony Triggers

Check `.ai-team/ceremonies.md` for automatic triggers:
- Design Review (before multi-agent work affecting shared systems)
- Retrospective (after work batches)

---

## Notes

- For ambiguous routing, ask Milo (Lead) first
- All agents may write decisions to their inbox files simultaneously (no conflicts)
- Ralph's work-check cycle runs after major work batches (if active)
