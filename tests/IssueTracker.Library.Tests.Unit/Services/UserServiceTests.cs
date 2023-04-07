namespace IssueTracker.CoreBusiness.Services;

[ExcludeFromCodeCoverage]
public class UserServiceTests
{
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private UserService _sut;

	public UserServiceTests()
	{
		_userRepositoryMock = new Mock<IUserRepository>();
		_sut = new UserService(_userRepositoryMock.Object);
	}

	[Fact(DisplayName = "Create User With Valid Values")]
	public async Task CreateUser_With_Valid_Values_Should_Return_Test()
	{
		// Arrange

		UserModel user = TestUsers.GetNewUser();

		_sut = new UserService(_userRepositoryMock.Object);

		// Act

		await _sut.CreateUser(user);

		// Assert

		_sut.Should().NotBeNull();

		_userRepositoryMock
			.Verify(x =>
				x.CreateUserAsync(It.IsAny<UserModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create User With Invalid User Throws Exception")]
	public async Task Create_With_Invalid_User_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new UserService(_userRepositoryMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateUser(null));
	}

	[Fact(DisplayName = "Get User With Valid Id")]
	public async Task GetUser_With_Valid_Id_Should_Return_Expected_User_Test()
	{
		//Arrange

		UserModel expected = TestUsers.GetKnownUser();

		_userRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_sut = new UserService(_userRepositoryMock.Object);

		//Act

		UserModel result = await _sut.GetUser(expected!.Id!);

		//Assert

		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Fact(DisplayName = "Get User With Empty String Id")]
	public async Task GetUser_With_Empty_String_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new UserService(_userRepositoryMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetUser(""));
	}

	[Fact(DisplayName = "Get User With Null Id")]
	public async Task GetUser_With_Null_Id_Should_Return_An_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new UserService(_userRepositoryMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetUser(null));
	}

	[Fact(DisplayName = "Get Users")]
	public async Task GetUsers_Should_Return_A_List_Of_Users_Test()
	{
		//Arrange

		const int expectedCount = 3;

		IEnumerable<UserModel> expected = TestUsers.GetUsers();

		_userRepositoryMock.Setup(x => x.GetUsersAsync()).ReturnsAsync(expected);

		_sut = new UserService(_userRepositoryMock.Object);

		//Act

		List<UserModel> results = await _sut.GetUsers();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get User From Authentication")]
	public async Task GetUserFromAuthentication_With_Valid_Authentication_Id_Should_Return_A_User_Test()
	{
		//Arrange

		UserModel expected = TestUsers.GetKnownUser();

		_userRepositoryMock.Setup(x => x.GetUserFromAuthenticationAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_sut = new UserService(_userRepositoryMock.Object);

		//Act

		UserModel result = await _sut.GetUserFromAuthentication(expected!.Id!);

		//Assert

		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Fact(DisplayName = "Get User From Authentication With Empty String")]
	public async Task GetUserFromAuthentication_With_Empty_String_Should_Return_A_ArgumentException_Test()
	{
		//Arrange

		_sut = new UserService(_userRepositoryMock.Object);

		//Act

		//Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetUserFromAuthentication(""));
	}

	[Fact(DisplayName = "Get User From Authentication With Null Value")]
	public async Task GetUserFromAuthentication_With_Null_Value_Should_Return_A_ArgumentNullException_Test()
	{
		//Arrange

		_sut = new UserService(_userRepositoryMock.Object);

		//Act

		//Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetUserFromAuthentication(null));
	}

	[Fact(DisplayName = "Update User With Valid User")]
	public async Task UpdateUser_With_A_Valid_User_Should_Succeed_Test()
	{
		// Arrange

		UserModel updatedUser = TestUsers.GetUpdatedUser();

		_sut = new UserService(_userRepositoryMock.Object);

		// Act

		await _sut.UpdateUser(updatedUser);

		// Assert

		_sut.Should().NotBeNull();

		_userRepositoryMock
			.Verify(x =>
				x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid User")]
	public async Task UpdateUser_With_Invalid_User_Should_Return_ArgumentNullException_Test()
	{
		// Arrange

		_sut = new UserService(_userRepositoryMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateUser(null));
	}
}