namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class EditSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public EditSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private EditSolutionUseCase CreateUseCase()
	{

		return new EditSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditSolutionUseCase With Valid Data Test")]
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
			x.UpdateSolutionAsync(It.IsAny<SolutionModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditSolutionUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel? solution = null;

		// Act
		await sut.ExecuteAsync(solution: solution);

		// Assert
		_solutionRepositoryMock.Verify(x =>
			x.UpdateSolutionAsync(It.IsAny<SolutionModel>()), Times.Never);

	}

}
