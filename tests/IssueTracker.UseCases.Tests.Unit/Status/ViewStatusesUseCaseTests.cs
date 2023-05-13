namespace IssueTracker.UseCases.Status;

[ExcludeFromCodeCoverage]
public class ViewStatusesUseCaseTests
{

	private readonly Mock<IStatusRepository> _statusRepositoryMock;

	public ViewStatusesUseCaseTests()
	{

		_statusRepositoryMock = new Mock<IStatusRepository>();

	}

	private ViewStatusesUseCase CreateUseCase(StatusModel expected)
	{

		var result = new List<StatusModel>
		{
			expected
		};

		_statusRepositoryMock.Setup(x => x.GetAllAsync(false))
			.ReturnsAsync(result);


		return new ViewStatusesUseCase(_statusRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewStatusesUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnAStatusModel_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetStatuses(1).First();
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync())!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);

		_statusRepositoryMock.Verify(x =>
			x.GetAllAsync(false), Times.Once);

	}

}
