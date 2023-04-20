namespace IssueTracker.UseCases.Tests.Unit.Users;

public class ViewUserByIdUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IUserRepository> mockUserRepository;

	public ViewUserByIdUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockUserRepository = this.mockRepository.Create<IUserRepository>();
	}

	private ViewUserByIdUseCase CreateViewUserByIdUseCase()
	{
		return new ViewUserByIdUseCase(
				this.mockUserRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewUserByIdUseCase = this.CreateViewUserByIdUseCase();
		string? id = null;

		// Act
		var result = await viewUserByIdUseCase.ExecuteAsync(
			id);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
