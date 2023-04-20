namespace IssueTracker.UseCases.Tests.Unit.Solution;

public class ArchiveSolutionUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<ISolutionRepository> mockSolutionRepository;

	public ArchiveSolutionUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockSolutionRepository = this.mockRepository.Create<ISolutionRepository>();
	}

	private ArchiveSolutionUseCase CreateArchiveSolutionUseCase()
	{
		return new ArchiveSolutionUseCase(
				this.mockSolutionRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var archiveSolutionUseCase = this.CreateArchiveSolutionUseCase();
		SolutionModel? solution = null;

		// Act
		await archiveSolutionUseCase.ExecuteAsync(
			solution);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
