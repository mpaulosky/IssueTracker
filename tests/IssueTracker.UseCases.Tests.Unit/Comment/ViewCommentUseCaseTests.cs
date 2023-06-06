namespace IssueTracker.UseCases.Comment;

[ExcludeFromCodeCoverage]
public class ViewCommentUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentUseCase CreateUseCase(CommentModel? expected)
	{

		if (expected != null)
		{
			_commentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(expected);
		}

		return new ViewCommentUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCommentByIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnACommentModel_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		var sut = CreateUseCase(expected);
		var commentId = expected.Id;

		// Act
		var result = await sut.ExecuteAsync(commentId);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_commentRepositoryMock.Verify(x =>
				x.GetAsync(It.IsAny<string>()), Times.Once);

	}

	[Theory(DisplayName = "ViewCommentByIdUseCase With In Valid Data Test")]
	[InlineData(null, "commentId", "Value cannot be null.?*")]
	[InlineData("", "commentId", "The value cannot be an empty string.?*")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentException_TestAsync(
		string? expectedId,
		string expectedParamName,
		string expectedMessage)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		Func<Task> act = async () => { await sut.ExecuteAsync(expectedId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);

	}

}
