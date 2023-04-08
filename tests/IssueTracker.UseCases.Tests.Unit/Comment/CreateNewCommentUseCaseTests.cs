namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class CreateNewCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public CreateNewCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private CreateNewCommentUseCase CreateUseCase()
	{

		return new CreateNewCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateNewCommentUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewComment_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		var comment = FakeComment.GetNewComment();

		// Act
		await _sut.Execute(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.CreateCommentAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateNewCommentUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewComment_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		CommentModel? comment = null;

		// Act
		await _sut.Execute(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.CreateCommentAsync(It.IsAny<CommentModel>()), Times.Never);

	}

}
