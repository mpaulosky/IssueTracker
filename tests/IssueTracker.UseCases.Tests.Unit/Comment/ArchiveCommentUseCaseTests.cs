namespace IssueTracker.UseCases.Tests.Unit.Comment;

public class ArchiveCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ArchiveCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ArchiveCommentUseCase CreateUseCase()
	{

		return new ArchiveCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ArchiveCommentUseCase With Valid Data Test")]
	public async Task ExecuteAsync_With_ValidData_Should_UpdateCommentAsArchived_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		CommentModel? comment = FakeComment.GetComments(1).First();

		// Act
		await _sut.Execute(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.UpdateCommentAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveCommentUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnNull_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase();
		CommentModel? comment = null;

		// Act
		await _sut.Execute(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.UpdateCommentAsync(It.IsAny<CommentModel>()), Times.Never);

	}

}
