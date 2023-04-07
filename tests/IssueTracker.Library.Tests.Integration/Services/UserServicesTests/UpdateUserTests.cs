using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateUserTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;
	private string _cleanupValue;

	public UpdateUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task UpdateUser_With_ValidData_Should_UpdateTheUser_Test()
	{

		// Arrange
		_cleanupValue = "users";
		UserModel expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		expected.DisplayName = "Updated";
		await _sut.UpdateUser(expected);
		UserModel result = await _sut.GetUser(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task UpdateUser_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task> act = async () => await _sut.UpdateUser(user: null);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

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
