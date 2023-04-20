namespace IssueTracker.UseCases.Tests.Unit.Status;

public class EditStatusUseCaseTests
{
	private readonly MockRepository mockRepository;

	private readonly Mock<IStatusRepository> mockStatusRepository;

	public EditStatusUseCaseTests()
	{
		this.mockRepository = new MockRepository(MockBehavior.Strict);

		this.mockStatusRepository = this.mockRepository.Create<IStatusRepository>();
	}

	private EditStatusUseCase CreateEditStatusUseCase()
	{
		return new EditStatusUseCase(
				this.mockStatusRepository.Object);
	}

	[Fact]
	public async Task ExecuteAsync_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		var editStatusUseCase = this.CreateEditStatusUseCase();
		StatusModel? status = null;

		// Act
		await editStatusUseCase.ExecuteAsync(
			status);

		// Assert
		Assert.True(false);
		this.mockRepository.VerifyAll();
	}
}
