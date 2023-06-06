namespace IssueTracker.UseCases.Comment;

[ExcludeFromCodeCoverage]
public class UpVoteCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public UpVoteCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private UpVoteCommentUseCase CreateUseCase()
	{

		return new UpVoteCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ExecuteAsync With Null CommentModel")]
	public async Task ExecuteAsync_With_Null_CommentModel_Should_Throw_ArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		const string expectedParamName = "comment";
		const string expectedMessage = "Value cannot be null.?*";
		CommentModel? comment = null;
		UserModel user = FakeUser.GetNewUser(true);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(comment, user); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

	[Fact(DisplayName = "ExecuteAsync With Null UserModel")]
	public async Task ExecuteAsync_With_Null_UserModel_Should_Throw_ArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		const string expectedParamName = "user";
		const string expectedMessage = "Value cannot be null.?*";
		CommentModel comment = FakeComment.GetNewComment(true);
		UserModel? user = null;

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(comment, user); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

	[Fact(DisplayName = "ExecuteAsync With Valid Inputs")]
	public async Task ExecuteAsync_With_Valid_Inputs_Should_Succeed_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase();
		CommentModel comment = FakeComment.GetNewComment(true);
		UserModel user = FakeUser.GetNewUser(true);

		// Act
		await sut.ExecuteAsync(comment, user);

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

	}

}
