namespace IssueTracker.UseCases.Tests.Unit.Status;

[ExcludeFromCodeCoverage]
public class EditStatusUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public EditStatusUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private EditStatusUseCase CreateUseCase()
	{

		return new EditStatusUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditStatusUseCase With Valid Data Test")]
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
			x.UpdateStatusAsync(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditStatusUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		StatusModel? status = null;

		// Act
		await sut.ExecuteAsync(status: status);

		// Assert
		_statusRepositoryMock.Verify(x =>
			x.UpdateStatusAsync(It.IsAny<StatusModel>()), Times.Never);

	}

}
