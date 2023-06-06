namespace IssueTracker.UseCases.Solution;

[ExcludeFromCodeCoverage]
public class ViewSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ViewSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ViewSolutionUseCase CreateUseCase(SolutionModel? expected)
	{

		if (expected != null)
		{
			_solutionRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewSolutionUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnASolutionModel_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var sut = CreateUseCase(expected);
		var solutionId = expected.Id;

		// Act
		var result = (await sut.ExecuteAsync(solutionId));

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_solutionRepositoryMock.Verify(x =>
			x.GetAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewSolutionUseCase With In Valid Data Test")]
	[InlineData(null, "solutionId", "Value cannot be null.?*")]
	[InlineData("", "solutionId", "The value cannot be an empty string.?*")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(
		string? expectedId,
		string expectedParamName,
		string expectedMessage)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(expectedId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
