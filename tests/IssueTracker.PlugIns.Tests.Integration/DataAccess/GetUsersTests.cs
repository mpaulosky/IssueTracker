namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUsersTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;
	private const string CleanupValue = "users";

	public GetUsersTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);

	}

	[Fact]
	public async Task GetAllAsync_With_ValidData_Should_ReturnUsers_Test()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateAsync(expected);

		// Act
		var results = (await _sut.GetAllAsync()).ToList();

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

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
