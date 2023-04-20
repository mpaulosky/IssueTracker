namespace IssueTracker.UseCases.Tests.Unit.Users;

public class CreateNewUserUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IUserRepository> mockUserRepository;

	public CreateNewUserUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
	}

	private CreateNewUserUseCase CreateCreateNewUserUseCase()
	{
		return new CreateNewUserUseCase(
				this.mockUserRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var createNewUserUseCase = this.CreateCreateNewUserUseCase();
		UserModel? user = null;

		// Act
		await createNewUserUseCase.ExecuteAsync(
			user);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
