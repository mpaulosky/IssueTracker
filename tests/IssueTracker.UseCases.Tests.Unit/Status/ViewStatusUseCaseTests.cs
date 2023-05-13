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
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		// Assert
		switch (expectedId)
		{
			case null:
				_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(expectedId!));
				break;
			case "":
				_ = await Assert.ThrowsAsync<ArgumentException>(() => sut.ExecuteAsync(expectedId));
				break;
		}
	}

}
