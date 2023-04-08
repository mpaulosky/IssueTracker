namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class EditCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public EditCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private EditCommentUseCase CreateUseCase()
	{

		return new EditCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditCommentUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditComment_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		CommentModel? comment = FakeComment.GetComments(1).First();
		comment.Title = "New Comment";

		// Act
		await _sut.Execute(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.UpdateCommentAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditCommentUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		CommentModel? comment = null;

		// Act
		await _sut.Execute(comment: comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.UpdateCommentAsync(It.IsAny<CommentModel>()), Times.Never);

	}

}
