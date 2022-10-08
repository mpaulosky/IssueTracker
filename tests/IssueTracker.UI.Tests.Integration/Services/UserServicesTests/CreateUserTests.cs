
namespace IssueTracker.UI.Tests.Integration.Services.UserServicesTests;

public class CreateUserTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly UserService _sut;

	public CreateUserTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task CreateUser_With_ValidData_Should_CreateAUser_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();

		// Act
		await _sut.CreateUser(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateUser_With_InValidData_Should_FailToCreateAUser_TestAsync()
	{

		// Arrange
		UserModel? expected = null;

		// Act
		var act = async () => await _sut.CreateUser(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}