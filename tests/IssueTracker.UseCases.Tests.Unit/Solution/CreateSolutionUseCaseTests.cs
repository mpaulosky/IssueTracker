namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class CreateSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public CreateSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private CreateSolutionUseCase CreateUseCase()
	{

		return new CreateSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateSolutionUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewSolution_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var solution = FakeSolution.GetNewSolution();

		// Act
		await sut.ExecuteAsync(solution);

		// Assert
		_solutionRepositoryMock.Verify(x =>
			x.CreateAsync(It.IsAny<SolutionModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateSolutionUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewSolution_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
