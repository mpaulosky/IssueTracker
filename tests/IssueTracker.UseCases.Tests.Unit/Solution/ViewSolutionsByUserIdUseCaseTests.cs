namespace IssueTracker.UseCases.Tests.Unit.Solution;

[ExcludeFromCodeCoverage]
public class ViewSolutionsByUserIdUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ViewSolutionsByUserIdUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ViewSolutionsByUserIdUseCase CreateUseCase(SolutionModel? expected)
	{
		if (expected == null)
		{
			return new ViewSolutionsByUserIdUseCase(_solutionRepositoryMock.Object);
		}

		var result = new List<SolutionModel>
		{
			expected
		};

		_solutionRepositoryMock.Setup(x => x.GetSolutionsByUserIdAsync(It.IsAny<string>()))
			.ReturnsAsync(result);

		return new ViewSolutionsByUserIdUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewSolutionsByUserIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnASolutionModel_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var expectedUser = FakeUser.GetNewUser();
		expectedUser.Id = expected.Author.Id;
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync(expectedUser)).First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_solutionRepositoryMock.Verify(x =>
			x.GetSolutionsByUserIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewSolutionsByUserIdUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		var result = await sut.ExecuteAsync(null);

		// Assert
		result.Should().BeNull();

		_solutionRepositoryMock.Verify(x =>
			x.GetSolutionsByUserIdAsync(It.IsAny<string>()), Times.Never);

	}

}
