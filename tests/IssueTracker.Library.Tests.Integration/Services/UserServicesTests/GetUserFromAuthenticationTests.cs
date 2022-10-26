namespace IssueTracker.Library.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetUserFromAuthenticationTests : IClassFixture<IssueTrackerTestFactory>
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;

	public GetUserFromAuthenticationTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUserFromAuthentication_With_ValidData_Should_ReturnAUser_Test()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		var result = await _sut.GetUserFromAuthentication(expected.ObjectIdentifier);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

}
