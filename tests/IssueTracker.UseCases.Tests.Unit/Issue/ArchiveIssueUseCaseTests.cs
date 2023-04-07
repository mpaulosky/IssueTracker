
namespace IssueTracker.UseCases.Tests.Unit.Issue;

public class ArchiveIssueUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IIssueRepository> mockIssueRepository;

	public ArchiveIssueUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockIssueRepository = this.mockRepository.Create<IIssueRepository>();
	}

	private ArchiveIssueUseCase CreateArchiveIssueUseCase()
	{
		return new ArchiveIssueUseCase(
				this.mockIssueRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var archiveIssueUseCase = this.CreateArchiveIssueUseCase();
		IssueModel? issue = null;

		// Act
		await archiveIssueUseCase.ExecuteAsync(
			issue);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
