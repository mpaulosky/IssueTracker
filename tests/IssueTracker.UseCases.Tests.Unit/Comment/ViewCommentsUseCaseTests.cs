namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class ViewCommentsUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentsUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentsUseCase CreateUseCase(CommentModel expected)
	{

		var result = new List<CommentModel>
			{
				expected
			};

		_commentRepositoryMock.Setup(x => x.GetCommentsAsync())
			.ReturnsAsync(result);


		return new ViewCommentsUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCommentsUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_ReturnACommentModel_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		var sut = CreateUseCase(expected);

		// Act
		var result = await sut.ExecuteAsync();

		// Assert
		result!.First().Should().NotBeNull();
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_commentRepositoryMock.Verify(x =>
				x.GetCommentsAsync(), Times.Once);

	}

}
