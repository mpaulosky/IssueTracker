namespace IssueTracker.UseCases.Solution;

[ExcludeFromCodeCoverage]
public class ViewSolutionsUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ViewSolutionsUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ViewSolutionsUseCase CreateUseCase(SolutionModel expected)
	{

		var result = new List<SolutionModel>
		{
			expected
		};

		_solutionRepositoryMock.Setup(x => x.GetAllAsync(false))
			.ReturnsAsync(result);


		return new ViewSolutionsUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewSolutionsUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnASolutionModel_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync())!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);

		_solutionRepositoryMock.Verify(x =>
			x.GetAllAsync(false), Times.Once);

	}

}
