namespace IssueTracker.UseCases.Tests.Unit.Status;

public class CreateNewStatusUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IStatusRepository> mockStatusRepository;

	public CreateNewStatusUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockStatusRepository = this.mockRepository.Create<IStatusRepository>();
	}

	private CreateNewStatusUseCase CreateCreateNewStatusUseCase()
	{
		return new CreateNewStatusUseCase(
				this.mockStatusRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var createNewStatusUseCase = this.CreateCreateNewStatusUseCase();
		StatusModel? status = null;

		// Act
		await createNewStatusUseCase.ExecuteAsync(
			status);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
