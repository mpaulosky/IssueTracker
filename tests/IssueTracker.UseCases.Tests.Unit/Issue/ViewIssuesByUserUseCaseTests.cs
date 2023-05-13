namespace IssueTracker.UseCases.Issue;

[ExcludeFromCodeCoverage]
public class ViewIssuesByUserUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssuesByUserUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssuesByUserUseCase CreateUseCase(IssueModel? expected)
	{

		if (expected != null)
		{

			var result = new List<IssueModel>
			{
				expected
			};

			_issueRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>()))
				.ReturnsAsync(result);

		}

		return new ViewIssuesByUserUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssuesByUserUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		var expectedUser = FakeUser.GetNewUser();
		expectedUser.Id = expected.Author.Id;
		var sut = CreateUseCase(expected);

		// Act
		var result = (await sut.ExecuteAsync(expectedUser))!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetByUserAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewIssuesByUserUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));

	}

}
