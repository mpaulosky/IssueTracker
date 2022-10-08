
namespace IssueTracker.UI.Tests.Integration.Services.UserServicesTests;

public class UserServiceTests : IClassFixture<IssueTrackerUIFactory>
{
	private readonly IssueTrackerUIFactory _factory;
	private IUserRepository _repo;

	public UserServiceTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		_repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));

	}

	[Fact]
	public void UserService_With_InvalidUserRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		var act = () => new UserService(_repo);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

}
