
namespace IssueTracker.UseCases.Tests.Unit.Issue;

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
				x.UpdateAsync(It.IsAny<IssueModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveIssueUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();

		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));

	}

}
