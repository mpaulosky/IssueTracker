namespace IssueTracker.UseCases.Status;

[ExcludeFromCodeCoverage]
public class ViewStatusUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public ViewStatusUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private ViewStatusUseCase CreateUseCase(StatusModel? expected)
	{

		if (expected != null)
		{
			_statusRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewStatusUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewStatusUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAStatusModel_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetStatuses(1).First();
		var sut = CreateUseCase(expected);
		var statusId = expected.Id;

		// Act
		var result = await sut.ExecuteAsync(statusId);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);

		_statusRepositoryMock.Verify(x =>
			x.GetAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewStatusUseCase With In Valid Data Test")]
	[InlineData(null, "statusId", "Value cannot be null.?*")]
	[InlineData("", "statusId", "The value cannot be an empty string.?*")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentException_TestAsync(
		string? expectedId,
		string expectedParamName,
		string expectedMessage)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(expectedId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
