namespace IssueTracker.UseCases.Solution;

[ExcludeFromCodeCoverage]
public class UpVoteSolutionUseCaseTests
{

	private readonly Mock<ISolutionRepository> _solutionRepositoryMock;

	public UpVoteSolutionUseCaseTests()
	{

		_solutionRepositoryMock = new Mock<ISolutionRepository>();

	}

	private UpVoteSolutionUseCase CreateUseCase()
	{

		return new UpVoteSolutionUseCase(_solutionRepositoryMock.Object);

	}

	[Fact(DisplayName = "ExecuteAsync With Null SolutionModel")]
	public async Task ExecuteAsync_With_Null_SolutionModel_Should_Throw_ArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel? solution = null;
		UserModel user = FakeUser.GetNewUser(true);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(solution, user); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName("solution")
			.WithMessage("Value cannot be null. (Parameter 'solution')");

	}

	[Fact(DisplayName = "ExecuteAsync With Null UserModel")]
	public async Task ExecuteAsync_With_Null_UserModel_Should_Throw_ArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel solution = FakeSolution.GetNewSolution(true);
		UserModel? user = null;

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(solution, user); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName("user")
			.WithMessage("Value cannot be null. (Parameter 'user')");

	}

	[Fact(DisplayName = "ExecuteAsync With Valid Inputs")]
	public async Task ExecuteAsync_With_Valid_Inputs_Should_Succeed_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		SolutionModel solution = FakeSolution.GetNewSolution(true);
		UserModel user = FakeUser.GetNewUser(true);

		// Act
		await sut.ExecuteAsync(solution, user);

		// Assert
		_solutionRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

	}

}
