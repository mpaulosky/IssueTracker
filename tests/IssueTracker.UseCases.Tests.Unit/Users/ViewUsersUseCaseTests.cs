namespace IssueTracker.UseCases.Tests.Unit.Users;

[ExcludeFromCodeCoverage]
public class ViewUsersUseCaseTests
{

	private readonly Mock<IUserRepository> _userRepositoryMock;

	public ViewUsersUseCaseTests()
	{

		_userRepositoryMock = new Mock<IUserRepository>();

	}

	private ViewUsersUseCase CreateUseCase(UserModel expected)
	{

		var result = new List<UserModel>
		{
			expected
		};

		_userRepositoryMock.Setup(x => x.GetAllAsync(false))
			.ReturnsAsync(result);


		return new ViewUsersUseCase(_userRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewUsersUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnAUserModel_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetUsers(1).First();
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync())!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.FirstName.Should().Be(expected.FirstName);
		result.LastName.Should().Be(expected.LastName);
		result.DisplayName.Should().Be(expected.DisplayName);

		_userRepositoryMock.Verify(x =>
			x.GetAllAsync(false), Times.Once);

	}

}
