namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateIssueTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;
	private const string CleanupValue = "issues";

	public CreateIssueTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new IssueRepository(context);

	}

	[Fact(DisplayName = "CreateAsync With Valid Data Should Succeed")]
	public async Task CreateAsync_With_Valid_Data_Should_Succeed_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();

		// Act
		await _sut.CreateAsync(expected).ConfigureAwait(false);
		IssueModel? result = await _sut.GetAsync(expected.Id).ConfigureAwait(false);

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
