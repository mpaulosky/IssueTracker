namespace IssueTracker.UI.Helpers;

[ExcludeFromCodeCoverage]
public class AuthenticationStateProviderHelpersTests : TestContext
{
	private readonly UserModel _expectedUser;
	private readonly Mock<AuthenticationStateProvider> _mockProvider;
	private readonly Mock<IUserService> _mockUserData;
	private AuthenticationState _authState = new(new ClaimsPrincipal());

	public AuthenticationStateProviderHelpersTests()
	{
		_expectedUser = FakeUser.GetNewUser(true);
		_mockProvider = new Mock<AuthenticationStateProvider>();
		_mockUserData = new Mock<IUserService>();
	}

	[Fact]
	public async Task GetUserFromAuth_Should_Call_GetAuthenticationStateAsync()
	{
		// Arrange
		_authState = AuthenticationStateFactory.Create(true, false, _expectedUser);
		SetupMocks();

		// Act
		UserModel result = await _mockProvider.Object.GetUserFromAuth(_mockUserData.Object);

		// Assert
		result.Should().BeEquivalentTo(_expectedUser);
		_mockProvider.Verify(x => x.GetAuthenticationStateAsync(), Times.Once);
	}

	[Fact]
	public async Task IsUserAuthorizedAsync_Should_Call_GetAuthenticationStateAsync()
	{
		// Arrange
		_authState = AuthenticationStateFactory.Create(true, false, _expectedUser);
		SetupMocks();

		// Act
		bool result = await _mockProvider.Object.IsUserAdminAsync();

		// Assert
		_mockProvider.Verify(x => x.GetAuthenticationStateAsync(), Times.Once);
	}

	[Fact]
	public async Task IsUserAuthorizedAsync_Should_Return_True_For_Admin_JobTitle()
	{
		// Arrange
		_authState = AuthenticationStateFactory.Create(true, true, _expectedUser);
		SetupMocks();

		// Act
		bool result = await _mockProvider.Object.IsUserAdminAsync();

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public async Task IsUserAuthorizedAsync_Should_Return_False_For_Normal_User_JobTitle()
	{
		// Arrange
		_authState = AuthenticationStateFactory.Create(true, false, _expectedUser);
		SetupMocks();

		// Act
		bool result = await _mockProvider.Object.IsUserAdminAsync();

		// Assert
		result.Should().BeFalse();
	}

	private void SetupMocks()
	{
		_mockUserData.Setup(x => x
				.GetUserFromAuthentication(It.IsAny<string>()))
			.ReturnsAsync(_expectedUser);

		_mockProvider.Setup(x => x
			.GetAuthenticationStateAsync()).ReturnsAsync(_authState);
	}
}