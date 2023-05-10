namespace IssueTracker.UseCases.Tests.Unit.Users;

[ExcludeFromCodeCoverage]
public class CreateUserUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public CreateUserUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private CreateUserUseCase CreateUseCase()
	{

		return new CreateUserUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateUserUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewUser_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var user = FakeUser.GetNewUser();

		// Act
		await sut.ExecuteAsync(user);

		// Assert
		_userRepositoryMock.Verify(x =>
			x.CreateAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateUserUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewUser_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
