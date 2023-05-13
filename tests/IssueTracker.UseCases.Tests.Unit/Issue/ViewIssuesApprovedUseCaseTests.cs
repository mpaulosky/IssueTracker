namespace IssueTracker.UseCases.Issue;

[ExcludeFromCodeCoverage]
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

		_issueRepositoryMock.Setup(x => x.GetApprovedAsync())
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
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync())!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetApprovedAsync(), Times.Once);

	}

}
