namespace IssueTracker.UseCases.Tests.Unit.Issue;

[ExcludeFromCodeCoverage]
public class ViewIssueByIdUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssueByIdUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssueByIdUseCase CreateUseCase(IssueModel? expected)
	{

		if (expected != null)
		{
			_issueRepositoryMock.Setup(x => x.GetIssueByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewIssueByIdUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssueByIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		var _sut = CreateUseCase(expected);
		var issueId = expected.Id;

		// Act
		var result = await _sut.ExecuteAsync(issueId);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetIssueByIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewIssueByIdUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{

		// Arrange
		var _sut = CreateUseCase(null);

		// Act
		var result = await _sut.ExecuteAsync(expectedId);

		// Assert
		result.Should().BeNull();

		_issueRepositoryMock.Verify(x =>
				x.GetIssueByIdAsync(It.IsAny<string>()), Times.Never);

	}

}
