namespace IssueTracker.UseCases.Tests.Unit.Status;

public class ViewStatusByIdUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IStatusRepository> mockStatusRepository;

	public ViewStatusByIdUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockStatusRepository = this.mockRepository.Create<IStatusRepository>();
	}

	private ViewStatusByIdUseCase CreateViewStatusByIdUseCase()
	{
		return new ViewStatusByIdUseCase(
				this.mockStatusRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var viewStatusByIdUseCase = this.CreateViewStatusByIdUseCase();
		string? statusId = null;

		// Act
		var result = await viewStatusByIdUseCase.ExecuteAsync(
			statusId);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
