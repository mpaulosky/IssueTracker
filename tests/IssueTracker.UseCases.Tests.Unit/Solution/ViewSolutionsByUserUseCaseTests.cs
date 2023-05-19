namespace IssueTracker.UseCases.Solution;

[ExcludeFromCodeCoverage]
public class ViewSolutionsByUserUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public ViewSolutionsByUserUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private ViewSolutionsByUserUseCase CreateUseCase(SolutionModel? expected)
	{
		if (expected == null)
		{
			return new ViewSolutionsByUserUseCase(_solutionRepositoryMock.Object);
		}

		var result = new List<SolutionModel>
		{
			expected
		};

		_solutionRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>()))
			.ReturnsAsync(result);

		return new ViewSolutionsByUserUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewSolutionsByUserUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnASolutionModel_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var expectedUser = FakeUser.GetNewUser();
		expectedUser.Id = expected.Author.Id;
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync(expectedUser.Id))!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_solutionRepositoryMock.Verify(x =>
			x.GetByUserAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewSolutionsByUserUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));

	}

}
