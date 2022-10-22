namespace IssueTracker.Library.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetCommentTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;

	public GetCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CommentService(repo, memCache);
	}

	[Fact]
	public async Task GetComment_With_WithData_Should_ReturnAValidComment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		var result = await _sut.GetComment(expected!.Id!);

		// Assert
		result.Id.Should().Be(expected!.Id);
		result.Comment.Should().BeEquivalentTo(expected!.Comment);
		result.Author.Should().BeEquivalentTo(expected!.Author);
		result.Issue.Should().BeEquivalentTo(expected!.Issue);

	}

	[Fact]
	public async Task GetComment_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		var id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetComment(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetComment_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		string id = null;

		// Act
		var act = async () => await _sut.GetComment(id!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetComment_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		var id = "";

		// Act
		var act = async () => await _sut.GetComment(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

	}

}
