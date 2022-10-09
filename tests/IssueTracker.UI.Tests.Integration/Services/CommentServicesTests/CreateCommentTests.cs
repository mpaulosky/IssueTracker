
namespace IssueTracker.UI.Tests.Integration.Services.CommentServicesTests;

public class CreateCommentTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly CommentService _sut;

	public CreateCommentTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task CreateComment_With_ValidData_Should_CreateAComment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();

		// Act
		await _sut.CreateComment(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateComment_With_InValidData_Should_FailToCreateAComment_TestAsync()
	{

		// Arrange
		CommentModel? expected = null;

		// Act
		var act = async () => await _sut.CreateComment(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}