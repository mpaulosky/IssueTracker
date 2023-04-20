namespace IssueTracker.UseCases.Tests.Unit.Users;

public class ViewUsersUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IUserRepository> mockUserRepository;

	public ViewUsersUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
	}

	private ViewUsersUseCase CreateViewUsersUseCase()
	{
		return new ViewUsersUseCase(
				this.mockUserRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewUsersUseCase = this.CreateViewUsersUseCase();

		// Act
		var result = await viewUsersUseCase.ExecuteAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
