namespace IssueTracker.UseCases.Tests.Unit.Status;

[ExcludeFromCodeCoverage]
public class UpdateStatusUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public UpdateStatusUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private UpdateStatusUseCase CreateUseCase()
	{

		return new UpdateStatusUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "UpdateStatusUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditStatus_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		StatusModel status = FakeStatus.GetStatuses(1).First();
		status.StatusName = "New Status";

		// Act
		await sut.ExecuteAsync(status);

		// Assert
		_statusRepositoryMock.Verify(x =>
			x.UpdateAsync(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "UpdateStatusUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
