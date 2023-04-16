namespace IssueTracker.PlugIns.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUserTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;
	private string _cleanupValue;

	public GetUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUser_With_WithData_Should_ReturnAValidUser_TestAsync()
	{

		// Arrange
		_cleanupValue = "users";
		var expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		var result = await _sut.GetUser(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task GetUser_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		_cleanupValue = "";
		const string id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetUser(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetUser_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task<UserModel>> act = async () => await _sut.GetUser(userId: null);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetUser_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		_cleanupValue = "";
		const string id = "";

		// Act
		Func<Task<UserModel>> act = async () => await _sut.GetUser(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

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
