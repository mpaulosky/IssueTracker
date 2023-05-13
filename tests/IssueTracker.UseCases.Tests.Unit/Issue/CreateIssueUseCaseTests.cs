namespace IssueTracker.UseCases.Issue;

[ExcludeFromCodeCoverage]
public class CreateIssueUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public CreateIssueUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private CreateIssueUseCase CreateUseCase()
	{

		return new CreateIssueUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateIssueUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewIssue_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var issue = FakeIssue.GetNewIssue();

		// Act
		await sut.ExecuteAsync(issue);

		// Assert
		_issueRepositoryMock.Verify(x =>
				x.CreateAsync(It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateIssueUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewIssue_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();

		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));

	}

}
