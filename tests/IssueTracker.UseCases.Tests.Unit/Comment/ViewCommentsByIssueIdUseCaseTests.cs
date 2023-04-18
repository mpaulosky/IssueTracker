namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class ViewCommentsBySourceUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentsBySourceUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentsBySourceUseCase CreateUseCase(CommentModel? expected)
	{

		if (expected != null)
		{

			var result = new List<CommentModel>
			{
				expected
			};

			_commentRepositoryMock.Setup(x => x.GetCommentsBySourceAsync(It.IsAny<BasicCommentOnSourceModel>()))
				.ReturnsAsync(result);

		}

		return new ViewCommentsBySourceUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCommentsBySourceUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnACommentModel_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		var _sut = CreateUseCase(expected);
		var source = expected.CommentOnSource;

		// Act
		var result = await _sut.ExecuteAsync(source);

		// Assert
		result.Should().NotBeNull();
		result!.Count().Should().Be(1);
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_commentRepositoryMock.Verify(x =>
				x.GetCommentsBySourceAsync(It.IsAny<BasicCommentOnSourceModel>()), Times.Once);

	}

	[Fact(DisplayName = "ViewCommentsBySourceUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var _sut = CreateUseCase(null);

		// Act
		var result = await _sut.ExecuteAsync(null);

		// Assert
		result.Should().BeNull();

		_commentRepositoryMock.Verify(x =>
				x.GetCommentsBySourceAsync(It.IsAny<BasicCommentOnSourceModel>()), Times.Never);

	}
}
