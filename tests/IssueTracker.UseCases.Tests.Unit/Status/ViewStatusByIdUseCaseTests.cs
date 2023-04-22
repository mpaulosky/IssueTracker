namespace IssueTracker.UseCases.Tests.Unit.Status;

[ExcludeFromCodeCoverage]
public class ViewStatusByIdUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public ViewStatusByIdUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private ViewStatusByIdUseCase CreateUseCase(StatusModel? expected)
	{

		if (expected != null)
		{
			_statusRepositoryMock.Setup(x => x.GetStatusByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewStatusByIdUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewStatusByIdUseCase With Valid Id Test")]
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
		result.Id.Should().Be(expected.Id);
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);

		_statusRepositoryMock.Verify(x =>
			x.GetStatusByIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewStatusByIdUseCase With In Valid Data Test")]
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		var result = await sut.ExecuteAsync(expectedId);

		// Assert
		result.Should().BeNull();

		_statusRepositoryMock.Verify(x =>
			x.GetStatusByIdAsync(It.IsAny<string>()), Times.Never);

	}

}
