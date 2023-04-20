namespace IssueTracker.UseCases.Tests.Unit.Solution;

public class ViewSolutionsByUserIdUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<ISolutionRepository> mockSolutionRepository;

	public ViewSolutionsByUserIdUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockSolutionRepository = this.mockRepository.Create<ISolutionRepository>();
	}

	private ViewSolutionsByUserIdUseCase CreateViewSolutionsByUserIdUseCase()
	{
		return new ViewSolutionsByUserIdUseCase(
				this.mockSolutionRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewSolutionsByUserIdUseCase = this.CreateViewSolutionsByUserIdUseCase();
		UserModel? user = null;

		// Act
		var result = await viewSolutionsByUserIdUseCase.ExecuteAsync(
			user);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
