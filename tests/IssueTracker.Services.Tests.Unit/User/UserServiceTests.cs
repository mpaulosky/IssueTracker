namespace IssueTracker.Services.User;

[ExcludeFromCodeCoverage]
public class UserServiceTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public UserServiceTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private UserService UnitUnderTest()
	{

		return new UserService(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "Create User With Valid Values")]
	public async Task CreateUser_With_Valid_Values_Should_Return_Test()
	{

		// Arrange
		var sut = UnitUnderTest();

		var user = FakeUser.GetNewUser();

		// Act
		await sut.CreateUser(user);

		// Assert
		sut.Should().NotBeNull();
		user.Id.Should().NotBeNull();

		_userRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "Create User With Invalid User Throws Exception")]
	public async Task Create_With_Invalid_User_Should_Return_ArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = UnitUnderTest();
		const string expectedParamName = "user";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.CreateUser(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

	[Fact(DisplayName = "Get User With Valid Id")]
	public async Task GetUser_With_Valid_Id_Should_Return_Expected_User_Test()
	{

		//Arrange
		var sut = UnitUnderTest();

		var expected = FakeUser.GetNewUser(true);

		_userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		var result = await sut.GetUser(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.EmailAddress.Should().Be(expected.EmailAddress);

	}

	[Theory(DisplayName = "Get User With Invalid Id")]
	[InlineData(null, "userId", "Value cannot be null.?*")]
	[InlineData("", "userId", "The value cannot be an empty string.?*")]
	public async Task GetUser_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string value, string expectedParamName, string expectedMessage)
	{

		// Arrange
		var sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetUser(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}


	[Fact(DisplayName = "Get Users")]
	public async Task GetUsers_Should_Return_A_List_Of_Users_Test()
	{
		//Arrange
		var sut = UnitUnderTest();

		const int expectedCount = 3;

		var expected = FakeUser.GetUsers(expectedCount);

		_userRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		//Act
		var results = await sut.GetUsers();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);

	}

	[Fact(DisplayName = "Get User From Authentication")]
	public async Task GetUserFromAuthentication_With_Valid_Authentication_Id_Should_Return_A_User_Test()
	{
		//Arrange
		var sut = UnitUnderTest();

		var expected = FakeUser.GetNewUser(true);

		_userRepositoryMock.Setup(x => x.GetFromAuthenticationAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		var result = await sut.GetUserFromAuthentication(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.EmailAddress.Should().Be(expected.EmailAddress);

	}

	[Theory(DisplayName = "Get User From Authentication With Invalid Data")]
	[InlineData(null, "userObjectIdentifierId", "Value cannot be null.?*")]
	[InlineData("", "userObjectIdentifierId", "The value cannot be an empty string.?*")]
	public async Task GetUserFromAuthentication_With_Invalid_Value_Should_Return_A_ArgumentException_Test(string value, string expectedParamName, string expectedMessage)
	{

		//Arrange
		var sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetUserFromAuthentication(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

	[Fact(DisplayName = "Update User With Valid User")]
	public async Task UpdateUser_With_A_Valid_User_Should_Succeed_Test()
	{

		// Arrange
		var sut = UnitUnderTest();

		var updatedUser = FakeUser.GetNewUser(true);

		// Act
		await sut.UpdateUser(updatedUser);

		// Assert
		sut.Should().NotBeNull();

		_userRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "Update With Invalid User")]
	public async Task UpdateUser_With_Invalid_User_Should_Return_ArgumentNullException_Test()
	{

		// Arrange
		var sut = UnitUnderTest();
		const string expectedParamName = "user";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.UpdateUser(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}
}
