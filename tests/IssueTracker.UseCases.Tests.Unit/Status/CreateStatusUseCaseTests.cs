namespace IssueTracker.UseCases.Status;

[ExcludeFromCodeCoverage]
public class CreateStatusUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public CreateStatusUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private CreateStatusUseCase CreateUseCase()
	{

		return new CreateStatusUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateStatusUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewStatus_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var status = FakeStatus.GetNewStatus();

		// Act
		await sut.ExecuteAsync(status);

		// Assert
		_statusRepositoryMock.Verify(x =>
			x.CreateAsync(It.IsAny<StatusModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateStatusUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "status";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
