namespace IssueTracker.Library.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class UserServiceTests : IClassFixture<IssueTrackerTestFactory>
{
	private readonly IssueTrackerTestFactory _factory;
	private IUserRepository _repo;

	public UserServiceTests(IssueTrackerTestFactory factory)
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
