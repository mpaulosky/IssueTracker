namespace IssueTracker.UseCases.Tests.Unit.Solution;

public class CreateNewSolutionUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<ISolutionRepository> mockSolutionRepository;

	public CreateNewSolutionUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockSolutionRepository = this.mockRepository.Create<ISolutionRepository>();
	}

	private CreateNewSolutionUseCase CreateCreateNewSolutionUseCase()
	{
		return new CreateNewSolutionUseCase(
				this.mockSolutionRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var createNewSolutionUseCase = this.CreateCreateNewSolutionUseCase();
		SolutionModel? solution = null;

		// Act
		await createNewSolutionUseCase.ExecuteAsync(
			solution);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
