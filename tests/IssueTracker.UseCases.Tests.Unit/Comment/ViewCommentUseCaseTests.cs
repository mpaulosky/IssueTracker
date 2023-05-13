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
	[InlineData(null)]
	[InlineData("")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
	{
		// Arrange
		var sut = CreateUseCase(null);

		// Act
		// Assert
		switch (expectedId)
		{
			case null:
				_ = await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ExecuteAsync(expectedId!));
				break;
			case "":
				_ = await Assert.ThrowsAsync<ArgumentException>(() => sut.ExecuteAsync(expectedId));
				break;
		}
	}

}
