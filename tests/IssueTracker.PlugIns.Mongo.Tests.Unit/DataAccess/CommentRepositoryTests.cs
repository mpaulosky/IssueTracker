namespace IssueTracker.CoreBusiness.DataAccess;

[ExcludeFromCodeCoverage]
public class CommentRepositoryTests
{
	private readonly Mock<IAsyncCursor<CommentModel>> _cursor;
	private readonly Mock<IMongoCollection<CommentModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly Mock<IMongoCollection<UserModel>> _mockUserCollection;
	private readonly Mock<IAsyncCursor<UserModel>> _userCursor;
	private List<CommentModel> _list = new();
	private CommentMongoRepository _sut;
	private List<UserModel> _users = new();

	public CommentRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);
		_userCursor = TestFixtures.GetMockCursor(_users);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection(_userCursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new CommentMongoRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Create Comment With Valid Comment")]
	public async Task CreateComment_With_A_Valid_Comment_Should_Return_Success_TestAsync()
	{
		// Arrange

		CommentModel newComment = TestComments.GetKnownComment();

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		UserModel user = TestUsers.GetKnownUser();
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new CommentMongoRepository(_mockContext.Object);

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

		CommentModel expected = TestComments.GetKnownComment();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentMongoRepository(_mockContext.Object);

		//Act

		CommentModel result = await _sut.GetCommentByIdAsync(expected!.Id!).ConfigureAwait(false);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.DateCreated.Should().NotBeBefore(Convert.ToDateTime("01/01/2000"));
		result.UserVotes.Should().NotBeNull();
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

		_sut = new CommentMongoRepository(_mockContext.Object);

		// Act

		IEnumerable<CommentModel> result = await _sut.GetCommentsAsync().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
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

		_sut = new CommentMongoRepository(_mockContext.Object);

		// Act

		IEnumerable<CommentModel> result = await _sut.GetCommentsByUserIdAsync(expectedUserId).ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(expectedCount);
		items[0].Author.Id.Should().NotBeNull();
		items[0].Author.DisplayName.Should().NotBeNull();
	}

	[Fact(DisplayName = "Update Comment")]
	public async Task UpdateComment_With_A_Valid_Id_And_Comment_Should_UpdateComment_TestAsync()
	{
		// Arrange

		CommentModel expected = TestComments.GetKnownComment();

		await _mockCollection.Object.InsertOneAsync(expected);

		CommentModel updatedComment =
			TestComments.GetComment(expected!.Id!, "Test Comment Update", expected!.Archived!);

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentMongoRepository(_mockContext.Object);

		// Act

		await _sut.UpdateCommentAsync(updatedComment);

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

		CommentModel expected = TestComments.GetKnownComment();

		await _mockCollection.Object.InsertOneAsync(expected);

		CommentModel updatedComment = TestComments.GetKnownComment();
		updatedComment.Archived = true;

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentMongoRepository(_mockContext.Object);

		// Act

		await _sut.UpdateCommentAsync(updatedComment);

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

		CommentModel expected = TestComments.GetKnownComment();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		UserModel user = TestUsers.GetKnownUserWithNoVotedOn();
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new CommentMongoRepository(_mockContext.Object);

		// Act

		await _sut.UpVoteCommentAsync(expected!.Id!, user!.Id!);

		// Assert

		expected.UserVotes.Count.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Upvote Comment With User Already Voted")]
	public async Task UpVoteComment_With_User_Already_Voted_Should_Remove_The_User_And_The_Comment_Test()
	{
		// Arrange

		CommentModel expected = TestComments.GetKnownComment();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		UserModel user = TestUsers.GetKnownUser();
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new CommentMongoRepository(_mockContext.Object);

		// Act

		await _sut.UpVoteCommentAsync(expected!.Id!, user!.Id!);

		// Assert

		expected.UserVotes.Count.Should().Be(0);
	}
}