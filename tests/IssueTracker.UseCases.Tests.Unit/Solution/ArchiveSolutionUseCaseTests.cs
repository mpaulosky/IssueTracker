namespace IssueTracker.UseCases.Solution;

[ExcludeFromCodeCoverage]
public class ArchiveSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ArchiveSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ArchiveSolutionUseCase CreateUseCase()
	{

		return new ArchiveSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ArchiveSolutionUseCase With Valid Data Test")]
	public async Task ExecuteAsync_With_ValidData_Should_UpdateSolutionAsArchived_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel solution = FakeSolution.GetSolutions(1).First();

		// Act
		await sut.ExecuteAsync(solution);

		// Assert
		_solutionRepositoryMock.Verify(x =>
			x.ArchiveAsync(It.IsAny<SolutionModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveSolutionUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();

		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));

	}

}
