using IssueTracker.PlugIns.Services;

namespace IssueTracker.PlugIns.Tests.Unit.Services;

[ExcludeFromCodeCoverage]
public class CommentServiceTests
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private CommentService _sut;

	public CommentServiceTests()
	{
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Create Comment With Valid Values")]
	public async Task CreateComment_With_Valid_Values_Should_Return_Test()
	{

		// Arrange

		var comment = TestComments.GetNewComment();

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.CreateComment(comment);

		// Assert

		_sut.Should().NotBeNull();

		_commentRepositoryMock
			.Verify(x =>
				x.CreateCommentAsync(It.IsAny<CommentModel>()), Times.Once);

	}

	[Fact(DisplayName = "Create Comment With Invalid Comment Throws Exception")]
	public async Task Create_With_Invalid_Comment_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateComment(null));
	}

	[Fact(DisplayName = "Get Comment With Valid Id")]
	public async Task GetComment_With_Valid_Id_Should_Return_Expected_Comment_Test()
	{
		//Arrange

		var expected = TestComments.GetKnownComment();

		_commentRepositoryMock.Setup(x => x.GetCommentAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var result = await _sut.GetComment(expected.Id!);

		//Assert

		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Fact(DisplayName = "Get Comment With Empty String Id")]
	public async Task GetComment_With_Empty_String_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetComment(""));
	}

	[Fact(DisplayName = "Get Comment With Null Id")]
	public async Task GetComment_With_Null_Id_Should_Return_An_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		_ = await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetComment(null));
	}

	[Fact(DisplayName = "Get Comments")]
	public async Task GetComments_Should_Return_A_List_Of_Comments_Test()
	{
		//Arrange

		const int expectedCount = 3;

		var expected = TestComments.GetComments();

		_commentRepositoryMock.Setup(x => x.GetCommentsAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetComments();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Comments with cache")]
	public async Task GetComments_With_Memory_Cache_Should_A_List_Of_Comments_Test()
	{
		//Arrange

		const int expectedCount = 3;

		var expected = TestComments.GetComments();


		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;

		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);
		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetComments();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Comments With Valid Id")]
	public async Task GetUsersComments_With_A_Valid_Id_Should_Return_A_List_Of_User_Comments_Test()
	{
		//Arrange

		const int expectedCount = 2;
		const string expectedUser = "5dc1039a1521eaa36835e541";

		var expected = TestComments.GetCommentsWithDuplicateAuthors()
			.Where(x => x.Author.Id == expectedUser).ToList();

		_commentRepositoryMock.Setup(x => x.GetCommentsByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetCommentsByUser(expectedUser);

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Comments with cache")]
	public async Task GetUsersComments_With_Memory_Cache_Should_Return_A_List_Of_User_Comments_Test()
	{
		//Arrange

		const int expectedCount = 2;
		const string expectedUser = "5dc1039a1521eaa36835e541";

		var expected = TestComments.GetCommentsWithDuplicateAuthors()
			.Where(x => x.Author.Id == expectedUser).ToList();

		_commentRepositoryMock.Setup(x => x.GetCommentsByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetCommentsByUser(expectedUser);

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Comments With empty string")]
	public async Task GetUsersComments_With_Empty_String_Users_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetCommentsByUser(""));
	}

	[Fact(DisplayName = "Get Users Comments With Null Id")]
	public async Task GetUsersComments_With_Null_Users_Id_Should_Return_An_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetCommentsByUser(null));
	}

	[Fact(DisplayName = "Update Comment With Valid Comment")]
	public async Task UpdateComment_With_A_Valid_Comment_Should_Succeed_Test()
	{
		// Arrange

		var updatedComment = TestComments.GetUpdatedComment();

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.UpdateComment(updatedComment);

		// Assert

		_sut.Should().NotBeNull();

		_commentRepositoryMock
			.Verify(x =>
				x.UpdateCommentAsync(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Comment")]
	public async Task UpdateComment_With_Invalid_Comment_Should_Return_ArgumentNullException_Test()
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateComment(null));
	}

	[Fact(DisplayName = "Up Vote Comment")]
	public async Task UpvoteComment_With_Valid_Inputs_Should_Be_Successful_Test()
	{
		// Arrange

		const string testId = "5dc1039a1521eaa36835e543";

		var comment = TestComments.GetKnownComment();

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.UpVoteComment(comment.Id!, testId);

		// Assert

		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteCommentAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Theory(DisplayName = "Upvote Comment With Invalid inputs")]
	[InlineData(null, "1")]
	[InlineData("1", null)]
	public async Task UpvoteComment_With_Invalid_Inputs_Should_Return_An_ArgumentNullException_TestAsync(
		string commentId,
		string userId)
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpVoteComment(commentId, userId));
	}

	[Theory(DisplayName = "Upvote Comment With Invalid inputs")]
	[InlineData("", "1")]
	[InlineData("1", "")]
	public async Task UpvoteComment_With_Invalid_Inputs_Should_Return_An_ArgumentException_TestAsync(
		string commentId,
		string userId)
	{
		// Arrange

		_sut = new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpVoteComment(commentId, userId));
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}
