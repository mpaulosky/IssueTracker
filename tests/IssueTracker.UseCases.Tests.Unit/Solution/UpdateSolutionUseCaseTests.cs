namespace IssueTracker.UseCases.Solution;

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
	public async Task Execute_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "solution";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
