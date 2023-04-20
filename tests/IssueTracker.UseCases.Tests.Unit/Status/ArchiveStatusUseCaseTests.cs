namespace IssueTracker.UseCases.Tests.Unit.Status;

public class ArchiveStatusUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IStatusRepository> mockStatusRepository;

	public ArchiveStatusUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockStatusRepository = this.mockRepository.Create<IStatusRepository>();
	}

	private ArchiveStatusUseCase CreateArchiveStatusUseCase()
	{
		return new ArchiveStatusUseCase(
				this.mockStatusRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var archiveStatusUseCase = this.CreateArchiveStatusUseCase();
		StatusModel? status = null;

		// Act
		await archiveStatusUseCase.ExecuteAsync(
			status);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
