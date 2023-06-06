namespace IssueTracker.UseCases.Comment;

[ExcludeFromCodeCoverage]
public class ViewCommentsByUserUseCaseTests
{

	private readonly Mock<ICommentRepository> _commentRepositoryMock;

	public ViewCommentsByUserUseCaseTests()
	{

		_commentRepositoryMock = new Mock<ICommentRepository>();

	}

	private ViewCommentsByUserUseCase CreateUseCase(CommentModel? expected)
	{

		if (expected != null)
		{

			var result = new List<CommentModel>
			{
				expected
			};

			_commentRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>()))
				.ReturnsAsync(result);

		}

		return new ViewCommentsByUserUseCase(_commentRepositoryMock.Object);

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
		var result = (await sut.ExecuteAsync(expectedUser))!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_commentRepositoryMock.Verify(x =>
				x.GetByUserAsync(It.IsAny<string>()), Times.Once);

	}

	[Fact(DisplayName = "ViewCommentsByUserIdUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);
		const string expectedParamName = "user";
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
