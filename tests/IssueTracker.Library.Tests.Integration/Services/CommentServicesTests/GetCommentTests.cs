using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Mongo.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;
	private string _cleanupValue;

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
		_cleanupValue = "comments";
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		CommentModel result = await _sut.GetComment(expected!.Id!);

		// Assert
		result.Id.Should().Be(expected!.Id);
		result.Title.Should().BeEquivalentTo(expected!.Title);
		result.Author.Should().BeEquivalentTo(expected!.Author);
		result.CommentOnSource.Should().BeEquivalentTo(expected!.CommentOnSource);

	}

	[Fact]
	public async Task GetComment_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		_cleanupValue = "";
		const string id = "62cf2ad6326e99d665759e5a";

		// Act
		CommentModel result = await _sut.GetComment(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetComment_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";

		// Act
		Func<Task<CommentModel>> act = async () => await _sut.GetComment(null);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetComment_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		_cleanupValue = "";
		const string id = "";

		// Act
		Func<Task<CommentModel>> act = async () => await _sut.GetComment(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}
}
