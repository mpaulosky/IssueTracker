
namespace IssueTracker.UI.Tests.Integration.Services.UserServicesTests;

public class GetUseresTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly UserService _sut;

	public GetUseresTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUseres_With_ValidData_Should_ReturnUseres_Test()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		var result = await _sut.GetUsers();

		// Assert
		result[0].Should().BeEquivalentTo(expected);

	}

}
