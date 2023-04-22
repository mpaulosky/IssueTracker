namespace IssueTracker.UseCases.Tests.Unit.Status;

[ExcludeFromCodeCoverage]
public class CreateNewStatusUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public CreateNewStatusUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private CreateNewStatusUseCase CreateUseCase()
	{

		return new CreateNewStatusUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateNewStatusUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewStatus_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var status = FakeStatus.GetNewStatus();

		// Act
		await sut.ExecuteAsync(status);

		// Assert
		_statusRepositoryMock.Verify(x =>
			x.CreateStatusAsync(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateNewStatusUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewStatus_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		StatusModel? status = null;

		// Act
		await sut.ExecuteAsync(status);

		// Assert
		_statusRepositoryMock.Verify(x =>
			x.CreateStatusAsync(It.IsAny<StatusModel>()), Times.Never);

	}

}
