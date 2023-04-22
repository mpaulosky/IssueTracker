namespace IssueTracker.UseCases.Tests.Unit.Issue;

[ExcludeFromCodeCoverage]
public class ViewIssuesUseCaseTests
{

	private readonly Mock<IIssueRepository> _issueRepositoryMock;

	public ViewIssuesUseCaseTests()
	{

		_issueRepositoryMock = new Mock<IIssueRepository>();

	}

	private ViewIssuesUseCase CreateUseCase(IssueModel expected)
	{

		var result = new List<IssueModel>
			{
				expected
			};

		_issueRepositoryMock.Setup(x => x.GetIssuesAsync())
			.ReturnsAsync(result);


		return new ViewIssuesUseCase(_issueRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewIssuesUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnAIssueModel_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		var sut = CreateUseCase(expected);

		// Act
		var result = await sut.ExecuteAsync();

		// Assert
		result!.First().Should().NotBeNull();
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_issueRepositoryMock.Verify(x =>
				x.GetIssuesAsync(), Times.Once);

	}

}
