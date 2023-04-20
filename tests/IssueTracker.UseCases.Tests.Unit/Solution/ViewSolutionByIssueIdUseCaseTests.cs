namespace IssueTracker.UseCases.Tests.Unit.Solution;

public class ViewSolutionByIssueIdUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<ISolutionRepository> mockSolutionRepository;

	public ViewSolutionByIssueIdUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockSolutionRepository = this.mockRepository.Create<ISolutionRepository>();
	}

	private ViewSolutionByIssueIdUseCase CreateViewSolutionByIssueIdUseCase()
	{
		return new ViewSolutionByIssueIdUseCase(
				this.mockSolutionRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewSolutionByIssueIdUseCase = this.CreateViewSolutionByIssueIdUseCase();
		IssueModel? issue = null;

		// Act
		var result = await viewSolutionByIssueIdUseCase.ExecuteAsync(
			issue);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
