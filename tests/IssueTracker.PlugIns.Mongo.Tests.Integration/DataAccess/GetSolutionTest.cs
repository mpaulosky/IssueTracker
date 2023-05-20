namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetSolutionTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly SolutionRepository _sut;
	private const string CleanupValue = "solutions";

	public GetSolutionTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new SolutionRepository(context);

	}

	[Fact(DisplayName = "GetAsync With Valid Data Should Succeed")]
	public async Task GetAsync_With_Valid_Data_Should_Succeed_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetNewSolution();
		await _sut.CreateAsync(expected).ConfigureAwait(false);

		// Act
		SolutionModel? result = await _sut.GetAsync(expected.Id).ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Archived.Should().Be(expected.Archived);

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
