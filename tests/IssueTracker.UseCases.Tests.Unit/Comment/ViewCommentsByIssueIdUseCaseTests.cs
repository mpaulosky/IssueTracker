namespace IssueTracker.UseCases.Tests.Unit.Comment
{
	public class ViewCommentsByIssueIdUseCaseTests
	{

		private readonly Mock<ICommentRepository> _commentRepositoryMock;

		public ViewCommentsByIssueIdUseCaseTests()
		{

			_commentRepositoryMock = new Mock<ICommentRepository>();

		}

		private ViewCommentsByIssueIdUseCase CreateUseCase(CommentModel? expected)
		{

			if (expected != null)
			{

				var result = new List<CommentModel>();
				result.Add(expected);

				_commentRepositoryMock.Setup(x => x.GetCommentsByIssueIdAsync(It.IsAny<string>()))
					.ReturnsAsync(result);

			}

			return new ViewCommentsByIssueIdUseCase(_commentRepositoryMock.Object);

		}

		[Fact(DisplayName = "ViewCommentsByIssueIdUseCase With Valid Id Test")]
		public async Task Execute_With_AValidId_Should_ReturnACommentModel_TestAsync()
		{

			// Arrange
			var expected = FakeComment.GetComments(1).First();
			var _sut = CreateUseCase(expected);
			var issueId = expected.CommentOnSource.Id;

			// Act
			var result = await _sut.ExecuteAsync(issueId);

			// Assert
			result.Should().NotBeNull();
			result.Count().Should().Be(1);
			result.First().Id.Should().Be(expected.Id);
			result.First().Title.Should().Be(expected.Title);
			result.First().Description.Should().Be(expected.Description);
			result.First().Author.Should().BeEquivalentTo(expected.Author);

			_commentRepositoryMock.Verify(x =>
					x.GetCommentByIdAsync(It.IsAny<string>()), Times.Once);

		}

		[Theory(DisplayName = "ViewCommentsByIssueIdUseCase With In Valid Data Test")]
		[InlineData(null)]
		[InlineData("")]
		public async Task ExecuteAsync_WithInValidData_ShouldReturnValidData_TestAsync(string? expectedId)
		{

			// Arrange
			var _sut = CreateUseCase(null);

			// Act
			var result = await _sut.ExecuteAsync(expectedId);

			// Assert
			result.Should().BeNull();


			_commentRepositoryMock.Verify(x =>
					x.GetCommentByIdAsync(It.IsAny<string>()), Times.Never);

		}
	}

}
