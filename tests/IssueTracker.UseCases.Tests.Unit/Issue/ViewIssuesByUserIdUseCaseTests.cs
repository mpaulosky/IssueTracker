namespace IssueTracker.UseCases.Tests.Unit.Issue;

public class ViewIssuesByUserIdUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssuesByUserIdUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssuesByUserIdUseCase CreateUseCase(IssueModel? expected)
	{

		if (expected != null)
		{

			var result = new List<IssueModel>
			{
				expected
			};

			_issueRepositoryMock.Setup(x => x.GetIssuesByUserIdAsync(It.IsAny<string>()))
				.ReturnsAsync(result);

		}

		return new ViewIssuesByUserIdUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssuesByUserIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		var expectedUser = FakeUser.GetNewUser();
		expectedUser.Id = expected.Author.Id;
		var _sut = CreateUseCase(expected);

		// Act
		var result = await _sut.ExecuteAsync(expectedUser);

		// Assert
		result!.First().Should().NotBeNull();
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetIssuesByUserIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewIssuesByUserIdUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase(null);

		// Act
		var result = await _sut.ExecuteAsync(null);

		// Assert
		result.Should().BeNull();

		_issueRepositoryMock.Verify(x =>
				x.GetIssuesByUserIdAsync(It.IsAny<string>()), Times.Never);

	}

}
