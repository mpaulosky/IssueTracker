namespace IssueTracker.Library.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetCommentsByUserTests : IClassFixture<IssueTrackerTestFactory>
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;

	public GetCommentsByUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("CommentsData");

		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task GetCommentByUser_With_ValidData_Should_ReturnValidComment_Test()
	{
		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		var result = await _sut.GetCommentsByUser(expected.Author.Id);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].Author.Id.Should().Be(expected.Author.Id);

	}

}
