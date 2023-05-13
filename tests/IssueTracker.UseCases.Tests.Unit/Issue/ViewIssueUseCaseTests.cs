namespace IssueTracker.UseCases.Issue;

[ExcludeFromCodeCoverage]
public class ViewIssueUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssueUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssueUseCase CreateUseCase(IssueModel? expected)
	{

		if (expected != null)
		{
			_issueRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewIssueUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssueUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		var sut = CreateUseCase(expected);
		var issueId = expected.Id;

		// Act
		var result = (await sut.ExecuteAsync(issueId));

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewIssueUseCase With In Valid Data Test")]
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
