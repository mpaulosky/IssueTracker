
namespace IssueTracker.UI.Tests.Integration.Services.CommentServicesTests;

public class UpdateCommentTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly CommentService _sut;

	public UpdateCommentTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task UpdateComment_With_ValidData_Should_UpdateTheComment_Test()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		expected.Comment = "Updated";
		await _sut.UpdateComment(expected);
		var result = await _sut.GetComment(expected!.Id!);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Fact]
	public async Task UpdateComment_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		CommentModel? expected = null;

		// Act
		var act = async () => await _sut.UpdateComment(expected!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}
