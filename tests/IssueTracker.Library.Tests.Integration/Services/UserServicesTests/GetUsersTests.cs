namespace IssueTracker.PlugIns.Mongo.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUsersTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;
	private string _cleanupValue;

	public GetUsersTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(UserModel)));

		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));

		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUsers_With_ValidData_Should_ReturnUsers_Test()
	{

		// Arrange
		_cleanupValue = "users";
		UserModel expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		List<UserModel> results = await _sut.GetUsers();

		// Assert
		results.Count.Should().Be(1);
		results.First().DisplayName.Should().Be(expected.DisplayName);
		results.First().FirstName.Should().Be(expected.FirstName);
		results.First().LastName.Should().Be(expected.LastName);

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}

}
