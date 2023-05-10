namespace IssueTracker.UseCases.Tests.Unit.Users;

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
			x.UpdateAsync(It.IsAny<UserModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveUserUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
