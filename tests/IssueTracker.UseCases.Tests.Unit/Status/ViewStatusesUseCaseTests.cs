namespace IssueTracker.UseCases.Tests.Unit.Status;

public class ViewStatusesUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IStatusRepository> mockStatusRepository;

	public ViewStatusesUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockStatusRepository = this.mockRepository.Create<IStatusRepository>();
	}

	private ViewStatusesUseCase CreateViewStatusesUseCase()
	{
		return new ViewStatusesUseCase(
				this.mockStatusRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewStatusesUseCase = this.CreateViewStatusesUseCase();

		// Act
		var result = await viewStatusesUseCase.ExecuteAsync();

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
