namespace IssueTracker.UseCases.Comment;

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
