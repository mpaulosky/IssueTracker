using IssueTracker.CoreBusiness.BogusFakes;

namespace IssueTracker.PlugIns.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class CommentRepositoryTests
{

	private readonly Mock<IAsyncCursor<CommentModel>> _cursor;
	private readonly Mock<IMongoCollection<CommentModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly Mock<IMongoCollection<UserModel>> _mockUserCollection;
	private readonly Mock<IAsyncCursor<UserModel>> _userCursor;
	private List<CommentModel> _list = new();
	private CommentRepository _sut;
	private List<UserModel> _users = new();

	public CommentRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);
		_userCursor = TestFixtures.GetMockCursor(_users);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection(_userCursor);

		_mockContext = GetMockMongoContext();

		_sut = new CommentRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Create Comment With Valid Comment")]
	public async Task CreateComment_With_A_Valid_Comment_Should_Return_Success_TestAsync()
	{
		// Arrange

		var newComment = TestComments.GetKnownComment();

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var user = TestUsers.GetKnownUser();
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		await _sut.CreateCommentAsync(newComment);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<CommentModel>(), null, default), Times.Once);
	}

	[Fact(DisplayName = "Get Comment With a Valid Id")]
	public async Task GetComment_With_Valid_Id_Should_Returns_One_Comment_TestAsync()
	{

		// Arrange
		var expected = TestComments.GetKnownComment();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		//Act
		CommentModel result = await _sut.GetCommentAsync(expected!.Id!).ConfigureAwait(false);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.DateCreated.Should().NotBeBefore(Convert.ToDateTime("01/01/2000"));
		result.UserVotes.Should().NotBeNull();

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Comments")]
	public async Task GetComments_With_Valid_Context_Should_Return_A_List_Of_Comments_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetComments().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_list = new List<CommentModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_sut = new CommentRepository(_mockContext.Object);

		// Act
		var result = (await _sut.GetCommentsAsync().ConfigureAwait(false)).ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(3);

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users Comments with valid Id")]
	public async Task GetUsersComments_With_Valid_Users_Id_Should_Return_A_List_Of_Users_Comments_TestAsync()
	{

		// Arrange
		const int expectedCount = 2;
		const string expectedUserId = "5dc1039a1521eaa36835e543";

		var expected = TestComments.GetComments().ToList();

		_list = new List<CommentModel>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act
		var result = (await _sut.GetCommentsByUserAsync(expectedUserId).ConfigureAwait(false)).ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(expectedCount);
		result[0].Author.Id.Should().NotBeNull();
		result[0].Author.DisplayName.Should().NotBeNull();

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Comment")]
	public async Task UpdateComment_With_A_Valid_Id_And_Comment_Should_UpdateComment_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedComment =
			TestComments.GetComment(expected!.Id!, "Test Comment Update", expected!.Archived!);

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		await _sut.UpdateCommentAsync(updatedComment.Id, updatedComment);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<CommentModel>>(), updatedComment,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Archive Comment")]
	public async Task ArchiveComment_With_A_Valid_Id_And_Comment_Should_ArchiveComment_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedComment = TestComments.GetKnownComment();
		updatedComment.Archived = true;

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		await _sut.ArchiveCommentAsync(updatedComment);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<CommentModel>>(), updatedComment,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Upvote Comment With Valid Comment and User")]
	public async Task UpVoteComment_With_A_Valid_CommentId_And_UserId_Should_Return_Success_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		var user = TestUsers.GetKnownUserWithNoVotedOn();
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		await _sut.UpVoteCommentAsync(expected!.Id!, user!.Id!);

		// Assert

		expected.UserVotes.Count.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Upvote Comment With User Already Voted")]
	public async Task UpVoteComment_With_User_Already_Voted_Should_Remove_The_User_And_The_Comment_Test()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		var user = TestUsers.GetKnownUser();
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		await _sut.UpVoteCommentAsync(expected!.Id!, user!.Id!);

		// Assert

		expected.UserVotes.Count.Should().Be(0);
	}

	[Fact(DisplayName = "Get Comments By Source")]
	public async Task GetCommentsBySourceAsync_With_ValidSource_Should_Return_A_List_Of_Comments_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		expected.Archived = false;

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		//Act
		var result = (await _sut.GetCommentsBySourceAsync(expected!.CommentOnSource!).ConfigureAwait(false)).ToList();

		//Assert 
		result.Should().NotBeNull();
		result.First().Should().BeEquivalentTo(expected);
		result.First().DateCreated.Should().NotBeBefore(Convert.ToDateTime("01/01/2000"));
		result.First().CommentOnSource.Should().NotBeNull();
		result.First().CommentOnSource.Should().BeEquivalentTo(expected.CommentOnSource);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}
}
