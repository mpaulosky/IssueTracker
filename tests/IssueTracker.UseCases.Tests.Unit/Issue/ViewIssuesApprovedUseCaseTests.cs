namespace IssueTracker.UseCases.Tests.Unit.Issue;

public class ViewIssuesApprovedUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssuesApprovedUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssuesApprovedUseCase CreateUseCase(IssueModel expected)
	{

		var result = new List<IssueModel>
			{
				expected
			};

		_issueRepositoryMock.Setup(x => x.GetIssuesApprovedAsync())
			.ReturnsAsync(result);


		return new ViewIssuesApprovedUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssuesApprovedUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		expected.ApprovedForRelease = true;
		expected.Rejected = false;
		var _sut = CreateUseCase(expected);

		// Act
		var result = await _sut.ExecuteAsync();

		// Assert
		result!.First().Should().NotBeNull();
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetIssuesApprovedAsync(), Times.Once);

	}

}
