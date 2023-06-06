﻿namespace IssueTracker.UseCases.Comment;

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

			_commentRepositoryMock.Setup(x => x.GetBySourceAsync(It.IsAny<BasicCommentOnSourceModel>()))
				.ReturnsAsync(result);

		}

		return new ViewCommentsBySourceUseCase(_commentRepositoryMock.Object);

	}

	[Fact(DisplayName = "ViewCommentsBySourceUseCase With Valid Id Test")]
	public async Task Execute_With_AValidId_Should_ReturnACommentModel_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		var sut = CreateUseCase(expected);
		var source = expected.CommentOnSource!;

		// Act
		var result = (await sut.ExecuteAsync(source))!.First();

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.Author.Should().BeEquivalentTo(expected.Author);

		_commentRepositoryMock.Verify(x =>
				x.GetBySourceAsync(It.IsAny<BasicCommentOnSourceModel>()), Times.Once);

	}

	[Fact(DisplayName = "ViewCommentsBySourceUseCase With In Valid Data Test")]
	public async Task ExecuteAsync_WithInValidData_ShouldReturnArgumentNullException_TestAsync()
	{

		// Arrange
		var sut = CreateUseCase(null);
		const string expectedParamName = "source";
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