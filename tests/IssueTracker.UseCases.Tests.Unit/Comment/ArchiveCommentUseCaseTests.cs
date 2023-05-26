namespace IssueTracker.UseCases.Comment;

[ExcludeFromCodeCoverage]
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
		var sut = CreateUseCase();
		CommentModel comment = FakeComment.GetComments(1).First();

		// Act
		await sut.ExecuteAsync(comment);

		// Assert
		_commentRepositoryMock.Verify(x =>
				x.ArchiveAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "ArchiveCommentUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_With_InValidData_Should_ReturnArgumentNullException_TestAsync()
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
