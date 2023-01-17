namespace IssueTracker.Library.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetIssuesByUserTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;
	private string _cleanupValue;

	public GetIssuesByUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetIssuesByUser_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		_cleanupValue = "issues";
		IssueModel expected = FakeIssue.GetNewIssue();
		await _sut.CreateIssue(expected);

		// Act
		List<IssueModel> results = await _sut.GetIssuesByUser(expected.Author.Id);

		// Assert
		results.Count.Should().Be(1);
		results.First().IssueName.Should().Be(expected.IssueName);
		results.First().Description.Should().Be(expected.Description);
		results.First().Author.Id.Should().Be(expected.Author.Id);

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}