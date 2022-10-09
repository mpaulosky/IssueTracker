
namespace IssueTracker.UI.Tests.Integration.Services.CommentServicesTests;

public class CommentServiceTests : IClassFixture<IssueTrackerUIFactory>
{
	private readonly IssueTrackerUIFactory _factory;
	private ICommentRepository _repo;
	private IMemoryCache _memCache;

	public CommentServiceTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		_repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		_memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
	}

	[Fact]
	public void CommentService_With_NullCommentRepository_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_repo = null;

		// Act
		var act = () => new CommentService(_repo, _memCache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	[Fact]
	public void CommentService_With_NullMemoryCache_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_memCache = null;

		// Act
		var act = () => new CommentService(_repo, _memCache);

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

}
