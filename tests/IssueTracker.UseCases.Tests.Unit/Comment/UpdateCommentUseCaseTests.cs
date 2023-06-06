namespace IssueTracker.UseCases.Comment;

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

	[Fact(DisplayName = "UpdateCommentUseCase With Valid Data Test")]
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

	[Fact(DisplayName = "UpdateCommentUseCase With In Valid Data Test")]
	public async Task Execute_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = this.CreateUseCase();
		const string expectedParamName = "comment";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
