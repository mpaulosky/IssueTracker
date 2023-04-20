namespace IssueTracker.UseCases.Tests.Unit.Solution;

public class ViewSolutionsUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<ISolutionRepository> mockSolutionRepository;

	public ViewSolutionsUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockSolutionRepository = this.mockRepository.Create<ISolutionRepository>();
	}

	private ViewSolutionsUseCase CreateViewSolutionsUseCase()
	{
		return new ViewSolutionsUseCase(
				this.mockSolutionRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewSolutionsUseCase = this.CreateViewSolutionsUseCase();

		// Act
		var result = await viewSolutionsUseCase.ExecuteAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
