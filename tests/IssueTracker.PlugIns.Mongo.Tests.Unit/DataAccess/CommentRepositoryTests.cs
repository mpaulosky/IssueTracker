namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
public class CommentRepositoryTests
{

	private readonly Mock<IAsyncCursor<CommentModel>> _cursor;
	private readonly Mock<IMongoCollection<CommentModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly Mock<IMongoCollection<UserModel>> _mockUserCollection;
	private readonly Mock<IAsyncCursor<UserModel>> _userCursor;
	private List<CommentModel> _list = new();
	private List<UserModel> _users = new();

	public CommentRepositoryTests()
	{

		_cursor = TestFixturesMongo.GetMockCursor(_list);
		_userCursor = TestFixturesMongo.GetMockCursor(_users);

		_mockCollection = TestFixturesMongo.GetMockCollection(_cursor);
		_mockUserCollection = TestFixturesMongo.GetMockCollection(_userCursor);

		_mockContext = TestFixturesMongo.GetMockContext();

	}

	private CommentRepository CreateRepository()
	{

		return new CommentRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Archive Comment Test")]
	public async Task ArchiveAsync_With_Valid_Comment_Should_Archive_the_Comment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment(true);

		var updatedComment = FakeComment.GetNewComment(true);
		updatedComment.Archived = true;

		await _mockCollection.Object.InsertOneAsync(expected);

		_mockContext.Setup(c => c
				.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedComment);

		// Assert

		_mockCollection.Verify(
			c => c.
				ReplaceOneAsync(It.IsAny<FilterDefinition<CommentModel>>(),
					updatedComment,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Create Comment With Valid Comment")]
	public async Task CreateAsync_With_A_Valid_Comment_Should_Return_Success_TestAsync()
	{

		// Arrange
		var newComment = FakeComment.GetNewComment();

		_mockContext.Setup(c => c
				.GetCollection<CommentModel>(It.IsAny<string>()))
				.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newComment);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(It.IsAny<CommentModel>(), null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Comment With a Valid Id")]
	public async Task GetAsync_With_Valid_Id_Should_Returns_One_Comment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<CommentModel>(It.IsAny<string>()))
				.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetAsync(expected.Id).ConfigureAwait(false);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result!.UserVotes.Should().NotBeNull();

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Comments Without Archived")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Comments_Without_Archived_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeComment.GetComments(expectedCount).ToList();
		foreach (var comment in expected)
		{
			comment.Archived = false;
		}

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
			.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<CommentModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync().ConfigureAwait(false))!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);
		results.Any(x => x.Archived).Should().BeFalse();

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Comments With Archived")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Comments_With_Archived_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeComment.GetComments(expectedCount).ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
				.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<CommentModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync(true).ConfigureAwait(false))!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);
		results.Any(x => x.Archived).Should().BeTrue();

		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Comments by Source")]
	public async Task GetBySourceAsync_With_Valid_Source_Should_Return_A_List_Of_Comments_TestAsync()
	{
		// Arrange
		const int expectedCount = 1;
		var expected = FakeComment.GetComments(expectedCount).First();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = (await sut.GetBySourceAsync(expected.CommentOnSource!).ConfigureAwait(false))!.First();

		// Assert
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);

		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users Comments with valid Id")]
	public async Task GetByUserAsync_With_Valid_Users_Id_Should_Return_A_List_Of_Users_Comments_TestAsync()
	{
		// Arrange
		const int expectedCount = 2;
		string expectedUserId = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
		var expectedAuthor = new BasicUserModel(expectedUserId, "jimjones");

		var expected = FakeComment.GetComments(expectedCount).ToList();
		expected[0].Author = expectedAuthor;
		expected[1].Author = expectedAuthor;

		_list = new List<CommentModel>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = (await sut.GetByUserAsync(expectedUserId).ConfigureAwait(false))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(expectedCount);
		result.Should().BeEquivalentTo(expected);
		result[0].Author.Id.Should().NotBeNull();
		result[0].Author.Id.Should().BeEquivalentTo(expectedUserId);
		result[1].Author.Id.Should().NotBeNull();
		result[1].Author.Id.Should().BeEquivalentTo(expectedUserId);

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<CommentModel>>(),
			It.IsAny<FindOptions<CommentModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Comment")]
	public async Task UpdateAsync_With_A_Valid_Id_And_Comment_Should_UpdateComment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();
		expected.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedComment = FakeComment.GetNewComment();
		updatedComment.Id = expected.Id;
		updatedComment.Archived = expected.Archived;
		updatedComment.Author = expected.Author;
		updatedComment.CommentOnSource = expected.CommentOnSource;
		updatedComment.DateCreated = expected.DateCreated;
		updatedComment.Description = "Updated Description";
		updatedComment.UserVotes = expected.UserVotes;
		updatedComment.Title = expected.Title;

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedComment);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<CommentModel>>(), updatedComment,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Up vote Comment With Valid Comment and User")]
	public async Task UpVoteCommentAsync_With_A_Valid_CommentId_And_UserId_Should_Return_Success_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		var user = FakeUser.GetNewUser(true);
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		var sut = CreateRepository();

		// Act
		await sut.UpVoteAsync(expected.Id, user.Id);

		// Assert
		expected.UserVotes.Count.Should().BeGreaterThan(0);

	}

	[Fact(DisplayName = "Up vote Comment With User Already Voted")]
	public async Task UpVoteComment_With_User_Already_Voted_Should_Remove_The_User_And_The_Comment_Test()
	{

		// Arrange
		var expected = FakeComment.GetComments(1).First();
		var expectedUser = FakeUser.GetUsers(1).First();
		expected.UserVotes.Add(expectedUser.Id);

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<CommentModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockUserCollection.Object);

		_users = new List<UserModel> { expectedUser };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		var sut = CreateRepository();

		// Act
		await sut.UpVoteAsync(expected.Id, expectedUser.Id);

		// Assert
		expected.UserVotes.Count.Should().Be(0);

	}

}
