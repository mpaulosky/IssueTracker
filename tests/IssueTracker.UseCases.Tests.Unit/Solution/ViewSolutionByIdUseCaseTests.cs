namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class ViewSolutionByIdUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ViewSolutionByIdUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ViewSolutionByIdUseCase CreateUseCase(SolutionModel? expected)
	{

		if (expected != null)
		{
			_solutionRepositoryMock.Setup(x => x.GetSolutionByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewSolutionByIdUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewSolutionByIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnASolutionModel_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var sut = CreateUseCase(expected);
		var solutionId = expected.Id;

		// Act
		var result = await sut.ExecuteAsync(solutionId);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result!.Title.Should().Be(expected.Title);
		result!.Description.Should().Be(expected.Description);
		result!.Author.Should().BeEquivalentTo(expected.Author);

		_solutionRepositoryMock.Verify(x =>
			x.GetSolutionByIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewSolutionByIdUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		var result = await sut.ExecuteAsync(expectedId);

		// Assert
		result.Should().BeNull();

		_solutionRepositoryMock.Verify(x =>
			x.GetSolutionByIdAsync(It.IsAny<string>()), Times.Never);

	}

}
