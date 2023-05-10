namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class UpdateSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public UpdateSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private UpdateSolutionUseCase CreateUseCase()
	{

		return new UpdateSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "UpdateSolutionUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditSolution_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel solution = FakeSolution.GetSolutions(1).First();
		solution.Title = "New Solution";

		// Act
		await sut.ExecuteAsync(solution);

		// Assert
		_solutionRepositoryMock.Verify(x =>
			x.UpdateAsync(It.IsAny<SolutionModel>()), Times.Once);

	}

	[Fact(DisplayName = "UpdateSolutionUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
