namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetIssuesByUserTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;
	private const string CleanupValue = "issues";

	public GetIssuesByUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new IssueRepository(context);

	}

	[Fact]
	public async Task GetByUserAsync_With_ValidData_Should_ReturnIssues_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		await _sut.CreateAsync(expected);

		// Act
		var results = (await _sut.GetByUserAsync(expected.Author.Id)).ToList();

		// Assert
		results.Count.Should().Be(1);
		results.First().Title.Should().Be(expected.Title);
		results.First().Description.Should().Be(expected.Description);
		results.First().Author.Id.Should().Be(expected.Author.Id);

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
