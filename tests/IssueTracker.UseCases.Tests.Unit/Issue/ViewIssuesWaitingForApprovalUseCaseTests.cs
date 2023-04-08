namespace IssueTracker.UseCases.Tests.Unit.Issue;

[ExcludeFromCodeCoverage]
public class ViewIssuesWaitingForApprovalUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssuesWaitingForApprovalUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssuesWaitingForApprovalUseCase CreateUseCase(IssueModel expected)
	{

		var result = new List<IssueModel>
			{
				expected
			};

		_issueRepositoryMock.Setup(x => x.GetIssuesWaitingForApprovalAsync())
			.ReturnsAsync(result);


		return new ViewIssuesWaitingForApprovalUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssuesWaitingForApprovalUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		expected.ApprovedForRelease = false;
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
				x.GetIssuesWaitingForApprovalAsync(), Times.Once);

	}

}
