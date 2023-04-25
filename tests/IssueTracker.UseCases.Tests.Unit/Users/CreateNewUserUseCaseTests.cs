namespace IssueTracker.UseCases.Tests.Unit.Users;

[ExcludeFromCodeCoverage]
public class CreateNewUserUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public CreateNewUserUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private CreateNewUserUseCase CreateUseCase()
	{

		return new CreateNewUserUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateNewUserUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewUser_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var user = FakeUser.GetNewUser();

		// Act
		await sut.ExecuteAsync(user);

		// Assert
		_userRepositoryMock.Verify(x =>
			x.CreateUserAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateNewUserUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewUser_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		UserModel? user = null;

		// Act
		await sut.ExecuteAsync(user);

		// Assert
		_userRepositoryMock.Verify(x =>
			x.CreateUserAsync(It.IsAny<UserModel>()), Times.Never);

	}

}
