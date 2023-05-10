namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class UpdateCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public UpdateCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private UpdateCommentUseCase CreateUseCase()
	{

		return new UpdateCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "EditCommentUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_EditComment_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		CommentModel comment = FakeComment.GetComments(1).First();
		comment.Title = "New Comment";

		// Act
		await sut.ExecuteAsync(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.UpdateAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "EditCommentUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
