namespace IssueTracker.Library.Services.IssueServiceTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetIssuesTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;
	private string _cleanupValue;

	public GetIssuesTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(IssueModel)));

		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("IssueData");

		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetIssues_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		_cleanupValue = "issues";
		IssueModel expected = FakeIssue.GetNewIssue();
		await _sut.CreateIssue(expected);

		// Act
		List<IssueModel> results = await _sut.GetIssues();

		// Assert
		results.Count.Should().Be(1);
		results.First().Title.Should().Be(expected.Title);
		results.First().Description.Should().Be(expected.Description);

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