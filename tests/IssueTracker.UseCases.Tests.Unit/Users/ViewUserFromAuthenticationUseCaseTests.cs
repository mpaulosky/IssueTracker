namespace IssueTracker.UseCases.Users;

[ExcludeFromCodeCoverage]
public class ViewUserFromAuthenticationUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public ViewUserFromAuthenticationUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private ViewUserFromAuthenticationUseCase CreateUseCase(UserModel? expected)
	{
		if (expected == null)
		{
			return new ViewUserFromAuthenticationUseCase(_userRepositoryMock.Object);
		}

		_userRepositoryMock.Setup(x =>
				x.GetByAuthenticationIdAsync(It.IsAny<string>()))
			.ReturnsAsync(expected);

		return new ViewUserFromAuthenticationUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewUserFromAuthenticationUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAUserModel_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetUsers(1).First();
		var sut = CreateUseCase(expected);
		var objectIdentifier = expected.ObjectIdentifier;

		// Act
		var result = await sut.ExecuteAsync(objectIdentifier);

		// Assert
		result.Should().NotBeNull();
		result!.ObjectIdentifier.Should().Be(expected.ObjectIdentifier);
		result.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.DisplayName.Should().Be(expected.DisplayName);

		_userRepositoryMock.Verify(x =>
			x.GetByAuthenticationIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewUserUseCase With In Valid Data Test")]
	[InlineData(null, "userObjectIdentifierId", "Value cannot be null.?*")]
	[InlineData("", "userObjectIdentifierId", "The value cannot be an empty string.?*")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentException_TestAsync(
		string? expectedId,
		string expectedParamName,
		string expectedMessage)
	{
		// Arrange
		var sut = this.CreateUseCase(null);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(expectedId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
