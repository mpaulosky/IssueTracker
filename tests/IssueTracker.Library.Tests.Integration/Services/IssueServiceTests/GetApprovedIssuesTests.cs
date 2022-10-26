namespace IssueTracker.Library.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetApprovedIssuesTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;

	public GetApprovedIssuesTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		expected.Rejected = false;
		expected.ApprovedForRelease = true;

		await _sut.CreateIssue(expected);

		// Act
		var results = await _sut.GetApprovedIssues();

		// Assert
		results.Count.Should().Be(1);
		results.First().IssueName.Should().Be(expected.IssueName);
		results.First().Description.Should().Be(expected.Description);

	}

}
