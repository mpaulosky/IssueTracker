namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class MongoDbContextFactoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private const string CleanupValue = "";

	public MongoDbContextFactoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		_factory.DbContext = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));

	}

	[Fact]
	public void GetCollection_With_Valid_DbContext_Should_Return_Value_Test()
	{

		// Arrange
		const string name = "users";

		// Act
		var result =
			_factory.DbContext!.GetCollection<UserModel>(name);

		// Assert
		result.Should().NotBeNull();

	}

	[Fact]
	public void ConnectionStateReturnsOpen()
	{

		// Given
		var client = _factory.DbContext!.Client;

		// When
		using var databases = client.ListDatabases();

		// Then
		Assert.Contains(databases.ToEnumerable(), database => database.TryGetValue("name", out var name) && "admin".Equals(name.AsString));

	}

	[Fact]
	public async Task Be_healthy_if_mongodb_is_available()
	{

		// Arrange
		var sut = _factory.Server;

		// Act
		var response = await sut.CreateRequest("/health").GetAsync();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

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
