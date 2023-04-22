namespace IssueTracker.UseCases.Tests.Unit.Users;

[ExcludeFromCodeCoverage]
public class EditUserUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public EditUserUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private EditUserUseCase CreateUseCase()
	{

		return new EditUserUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditUserUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditUser_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		UserModel user = FakeUser.GetUsers(1).First();
		user.FirstName = "James";

		// Act
		await sut.ExecuteAsync(user);

		// Assert
		_userRepositoryMock.Verify(x =>
			x.UpdateUserAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditUserUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		UserModel? user = null;

		// Act
		await sut.ExecuteAsync(user: user);

		// Assert
		_userRepositoryMock.Verify(x =>
			x.UpdateUserAsync(It.IsAny<UserModel>()), Times.Never);

	}

}
