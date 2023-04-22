namespace IssueTracker.UseCases.Tests.Unit.Users;

[ExcludeFromCodeCoverage]
public class ViewUserByIdUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public ViewUserByIdUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private ViewUserByIdUseCase CreateUseCase(UserModel? expected)
	{

		if (expected != null)
		{
			_userRepositoryMock.Setup(x => x.GetUserByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewUserByIdUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewUserByIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAUserModel_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetUsers(1).First();
		var sut = CreateUseCase(expected);
		var userId = expected.Id;

		// Act
		var result = await sut.ExecuteAsync(userId);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.DisplayName.Should().Be(expected.DisplayName);

		_userRepositoryMock.Verify(x =>
			x.GetUserByIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewUserByIdUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		var result = await sut.ExecuteAsync(expectedId);

		// Assert
		result.Should().BeNull();

		_userRepositoryMock.Verify(x =>
			x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);

	}

}
