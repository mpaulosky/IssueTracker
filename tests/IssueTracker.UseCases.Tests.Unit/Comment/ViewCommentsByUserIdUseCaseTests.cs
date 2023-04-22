namespace IssueTracker.UseCases.Tests.Unit.Comment;

[ExcludeFromCodeCoverage]
public class ViewCommentsByUserIdUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentsByUserIdUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentsByUserIdUseCase CreateUseCase(CommentModel? expected)
	{

		if (expected != null)
		{

			var result = new List<CommentModel>
			{
				expected
			};

			_commentRepositoryMock.Setup(x => x.GetCommentsByUserIdAsync(It.IsAny<string>()))
				.ReturnsAsync(result);

		}

		return new ViewCommentsByUserIdUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCommentsByUserIdUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnACommentModel_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		var expectedUser = FakeUser.GetNewUser();
		expectedUser.Id = expected.Author.Id;
		var sut = CreateUseCase(expected);

		// Act
		var result = await sut.ExecuteAsync(expectedUser);

		// Assert
		result!.First().Should().NotBeNull();
		result!.First().Id.Should().Be(expected.Id);
		result!.First().Title.Should().Be(expected.Title);
		result!.First().Description.Should().Be(expected.Description);
		result!.First().Author.Should().BeEquivalentTo(expected.Author);

		_commentRepositoryMock.Verify(x =>
				x.GetCommentsByUserIdAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewCommentsByUserIdUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);

		// Act
		var result = await sut.ExecuteAsync(null);

		// Assert
		result.Should().BeNull();

		_commentRepositoryMock.Verify(x =>
				x.GetCommentsByUserIdAsync(It.IsAny<string>()), Times.Never);

	}

}
