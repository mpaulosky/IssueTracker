namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetByIssueSolutionTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly SolutionRepository _sut;
	private const string? CleanupValue = "solutions";

	public GetByIssueSolutionTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new SolutionRepository(context);

	}

	[Fact]
	public async Task GetBySourceAsync_With_Valid_Data_Should_Be_Successful_TestAsync()
	{
		// Arrange
		var items = FakeSolution.GetSolutions(3).ToList();
		var source = items[0].Issue;

		foreach (var issue in items)
		{
			issue.Id = string.Empty;
			issue.Issue.Id = source.Id;
			await _sut.CreateAsync(issue);
		}

		// Act
		var result = (await _sut.GetByIssueAsync(source.Id))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(1);
		result[0].Issue.Id.Should().Be(source.Id);

	}

	public Task InitializeAsync()
	{

		return Task.CompletedTask;

	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
