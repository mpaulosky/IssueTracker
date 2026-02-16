# History — Hooper

## Project Learnings (from init)

**Project:** IssueTracker  
**Test Stack:** xUnit, bUnit, FluentAssertions, NSubstitute, TestContainers  
**Owner:** mpaulosky  
**Goal:** Aspireify and enhance with robust test coverage  

**xUnit Patterns:**
- Test method: `MethodName_Condition_ExpectedResult`
- Assertions: FluentAssertions (`x.Should().Be(expected)`, `x.Should().Throw<Exception>()`)
- Mocks: NSubstitute (`Substitute.For<IInterface>()`)
- Arrange-Act-Assert (AAA) pattern
- File-scoped namespaces, PascalCase test classes

**bUnit Patterns:**
- Component tests in `.razor.cs` files
- Render component: `var cut = RenderComponent<IssueComponent>(parameters => ...)`
- Test interactions: `cut.Find("button").Click()`
- Snapshot tests: `await cut.VerifyAsync()`
- Event callbacks: `cut.Instance.OnSubmit` setup

**Integration Tests:**
- TestContainers: Docker MongoDB for isolation
- Build container in test setup, clean up in teardown
- Test full flow: API → Service → Repository → Database

**Coverage Goals:**
- >80% coverage on business logic
- 100% on critical paths
- All public APIs: ≥1 integration test
- All components: ≥1 bUnit test

## Learnings

(None yet — first session)
