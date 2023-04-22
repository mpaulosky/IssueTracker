namespace IssueTracker.UseCases.Tests.Unit.Issue;

[ExcludeFromCodeCoverage]
public class EditIssueUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public EditIssueUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private EditIssueUseCase CreateUseCase()
	{

		return new EditIssueUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditIssueUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditIssue_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		IssueModel? issue = FakeIssue.GetIssues(1).First();
		issue.Title = "New Issue";

		// Act
		await sut.ExecuteAsync(issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.UpdateIssueAsync(It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditIssueUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		IssueModel? issue = null;

		// Act
		await sut.ExecuteAsync(issue: issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.UpdateIssueAsync(It.IsAny<IssueModel>()), Times.Never);

	}

}
