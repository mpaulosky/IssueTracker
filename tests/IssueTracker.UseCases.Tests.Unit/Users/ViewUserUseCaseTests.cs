namespace IssueTracker.UseCases.Tests.Unit.Users;

[ExcludeFromCodeCoverage]
public class ViewUserUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public ViewUserUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private ViewUserUseCase CreateUseCase(UserModel? expected)
	{

		if (expected != null)
		{
			_userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewUserUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewUserUseCase With Valid Id Test")]
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
		result!.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.DisplayName.Should().Be(expected.DisplayName);

		_userRepositoryMock.Verify(x =>
			x.GetAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewUserUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{
		// Arrange
		var sut = CreateUseCase(null);
		
		// Act
		// Assert
		switch (expectedId)
		{
			case null:
				_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(expectedId!));
				break;
			case "":
				_ = await Assert.ThrowsAsync<ArgumentException>(() => sut.ExecuteAsync(expectedId));
				break;
		}
	}


}
