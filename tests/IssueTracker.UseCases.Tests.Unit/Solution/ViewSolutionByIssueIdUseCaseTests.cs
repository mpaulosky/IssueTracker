namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class ViewSolutionsByIssueIdUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ViewSolutionsByIssueIdUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ViewSolutionsByIssueIdUseCase CreateUseCase(SolutionModel? expected)
	{
		if (expected == null)
		{
			return new ViewSolutionsByIssueIdUseCase(_solutionRepositoryMock.Object);
		}

		var result = new List<SolutionModel>
		{
			expected
		};

		_solutionRepositoryMock.Setup(x => x
				.GetSolutionsByIssueIdAsync(It.IsAny<string>()))
			.ReturnsAsync(result);

		return new ViewSolutionsByIssueIdUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewSolutionsBySourceUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnASolutionModel_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var sut = CreateUseCase(expected);
		var issue = expected.Issue;

		// Act
		var result = await sut.ExecuteAsync(issue);

		// Assert
		result.Should().NotBeNull();
		result!.Count().Should().Be(1);
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_solutionRepositoryMock.Verify(x =>
			x.GetSolutionsByIssueIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewSolutionsBySourceUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		var result = await sut.ExecuteAsync(null);

		// Assert
		result.Should().BeNull();

		_solutionRepositoryMock.Verify(x =>
			x.GetSolutionsByIssueIdAsync(It.IsAny<string>()), Times.Never);

	}
}
