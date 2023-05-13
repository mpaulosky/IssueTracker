namespace IssueTracker.UseCases.Status;

[ExcludeFromCodeCoverage]
public class ArchiveStatusUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public ArchiveStatusUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private ArchiveStatusUseCase CreateUseCase()
	{

		return new ArchiveStatusUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "ArchiveStatusUseCase With Valid Data Test")]
	public async Task ExecuteAsync_With_ValidData_Should_UpdateStatusAsArchived_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		StatusModel status = FakeStatus.GetStatuses(1).First();

		// Act
		await sut.ExecuteAsync(status);

		// Assert
		_statusRepositoryMock.Verify(x =>
			x.UpdateAsync(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveStatusUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();

		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));

	}

}
