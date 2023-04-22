namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class CreateNewSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public CreateNewSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private CreateNewSolutionUseCase CreateUseCase()
	{

		return new CreateNewSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateNewSolutionUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewSolution_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var solution = FakeSolution.GetNewSolution();

		// Act
		await sut.ExecuteAsync(solution);

		// Assert
		_solutionRepositoryMock.Verify(x =>
			x.CreateSolutionAsync(It.IsAny<SolutionModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateNewSolutionUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewSolution_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel? solution = null;

		// Act
		await sut.ExecuteAsync(solution);

		// Assert
		_solutionRepositoryMock.Verify(x =>
			x.CreateSolutionAsync(It.IsAny<SolutionModel>()), Times.Never);

	}

}
