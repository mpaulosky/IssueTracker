namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class CreateCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public CreateCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private CreateCommentUseCase CreateUseCase()
	{

		return new CreateCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "CreateNewCommentUseCase With Valid Data Test")]
	public async Task Execute_With_ValidData_Should_CreateANewComment_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		var comment = FakeComment.GetNewComment();

		// Act
		await sut.ExecuteAsync(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.CreateAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "CreateNewCommentUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_CreateANewComment_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		
		// Act
		// Assert
		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(null!));
		
	}

}
