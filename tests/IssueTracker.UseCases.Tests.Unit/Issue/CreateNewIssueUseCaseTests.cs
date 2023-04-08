namespace IssueTracker.UseCases.Tests.Unit.Issue;

[ExcludeFromCodeCoverage]
public class CreateNewIssueUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public CreateNewIssueUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private CreateNewIssueUseCase CreateUseCase()
	{

		return new CreateNewIssueUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateNewIssueUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewIssue_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		var issue = FakeIssue.GetNewIssue();

		// Act
		await _sut.ExecuteAsync(issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.CreateIssueAsync(It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateNewIssueUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewIssue_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		IssueModel? issue = null;

		// Act
		await _sut.ExecuteAsync(issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.CreateIssueAsync(It.IsAny<IssueModel>()), Times.Never);

	}

}
