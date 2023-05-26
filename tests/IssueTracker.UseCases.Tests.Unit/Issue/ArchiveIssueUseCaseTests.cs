namespace IssueTracker.UseCases.Issue;

[ExcludeFromCodeCoverage]
public class ArchiveIssueUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ArchiveIssueUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ArchiveIssueUseCase CreateUseCase()
	{

		return new ArchiveIssueUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ArchiveIssueUseCase With Valid Data Test")]
	public async Task ExecuteAsync_With_ValidData_Should_UpdateIssueAsArchived_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		IssueModel issue = FakeIssue.GetIssues(1).First();

		// Act
		await sut.ExecuteAsync(issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.ArchiveAsync(It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveIssueUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "issue";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
