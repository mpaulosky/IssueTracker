namespace IssueTracker.UseCases.Users;

[ExcludeFromCodeCoverage]
public class ArchiveUserUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public ArchiveUserUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private ArchiveUserUseCase CreateUseCase()
	{

		return new ArchiveUserUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "ArchiveUserUseCase With Valid Data Test")]
	public async Task ExecuteAsync_With_ValidData_Should_UpdateUserAsArchived_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		UserModel user = FakeUser.GetUsers(1).First();

		// Act
		await sut.ExecuteAsync(user);

		// Assert
		_userRepositoryMock.Verify(x =>
			x.ArchiveAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveUserUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
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
