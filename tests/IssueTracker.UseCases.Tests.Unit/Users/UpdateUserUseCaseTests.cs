namespace IssueTracker.UseCases.Users;

[ExcludeFromCodeCoverage]
public class UpdateUserUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public UpdateUserUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private UpdateUserUseCase CreateUseCase()
	{

		return new UpdateUserUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "UpdateUserUseCase With Valid Data Test")]
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
			x.UpdateAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "UpdateUserUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "user";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
