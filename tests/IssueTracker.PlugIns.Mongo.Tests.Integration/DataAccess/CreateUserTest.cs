namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateUserTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;
	private const string CleanupValue = "users";

	public CreateUserTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);

	}

	[Fact(DisplayName = "CreateAsync With Valid Data Should Succeed")]
	public async Task CreateAsync_With_Valid_Data_Should_Succeed_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();

		// Act
		await _sut.CreateAsync(expected).ConfigureAwait(false);
		UserModel? result = await _sut.GetAsync(expected.Id).ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.EmailAddress.Should().Be(expected.EmailAddress);
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
