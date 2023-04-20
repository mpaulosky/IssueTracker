namespace IssueTracker.UseCases.Tests.Unit.Solution;

public class EditSolutionUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<ISolutionRepository> mockSolutionRepository;

	public EditSolutionUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockSolutionRepository = this.mockRepository.Create<ISolutionRepository>();
	}

	private EditSolutionUseCase CreateEditSolutionUseCase()
	{
		return new EditSolutionUseCase(
				this.mockSolutionRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var editSolutionUseCase = this.CreateEditSolutionUseCase();
		SolutionModel? solution = null;

		// Act
		await editSolutionUseCase.ExecuteAsync(
			solution);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
