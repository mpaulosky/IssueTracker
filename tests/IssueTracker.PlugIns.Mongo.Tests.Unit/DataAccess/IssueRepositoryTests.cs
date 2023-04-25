namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class IssueRepositoryTests
{

	private readonly Mock<IAsyncCursor<IssueModel>> _cursor;
	private readonly Mock<IMongoCollection<IssueModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly Mock<IMongoCollection<UserModel>> _mockUserCollection;
	private readonly Mock<IAsyncCursor<UserModel>> _userCursor;
	private List<IssueModel> _list = new();
	private readonly List<UserModel> _users = new();

	public IssueRepositoryTests()
	{

		_cursor = TestFixtures.GetMockCursor(_list);
		_userCursor = TestFixtures.GetMockCursor(_users);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection(_userCursor);

		_mockContext = TestFixtures.GetMockContext();

	}

	private IssueRepository CreateRepository()
	{

		return new IssueRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Create Issue With Valid Issue")]
	public async Task CreateIssueAsync_With_A_Valid_Issue_Should_Return_Success_TestAsync()
	{

		// Arrange
		var newIssue = FakeIssue.GetNewIssue();

		_mockContext.Setup(c => c
				.GetCollection<IssueModel>(It.IsAny<string>()))
				.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateIssueAsync(newIssue);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(It.IsAny<IssueModel>(), null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Issue With a Valid Id")]
	public async Task GetIssueByIdAsync_With_Valid_Id_Should_Returns_One_Issue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<IssueModel>(It.IsAny<string>()))
				.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetIssueByIdAsync(expected!.Id!).ConfigureAwait(false);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.DateCreated.Should().NotBeBefore(Convert.ToDateTime("01/01/2000"));

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssuesAsync_With_Valid_Context_Should_Return_A_List_Of_Issues_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeIssue.GetIssues(expectedCount).ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
			.GetCollection<IssueModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<IssueModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetIssuesAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users Issues with valid Id")]
	public async Task GetIssuesByUserIdAsync_With_Valid_Users_Id_Should_Return_A_List_Of_Users_Issues_TestAsync()
	{
		// Arrange
		const int expectedCount = 2;
		string expectedUserId = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
		var expectedAuthor = new BasicUserModel(expectedUserId, "jimjones");

		var expected = FakeIssue.GetIssues(expectedCount).ToList();
		expected[0].Author = expectedAuthor;
		expected[1].Author = expectedAuthor;

		_list = new List<IssueModel>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<IssueModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = (await sut.GetIssuesByUserIdAsync(expectedUserId).ConfigureAwait(false)).ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(expectedCount);
		result.Should().BeEquivalentTo(expected);
		result[0].Author.Id.Should().NotBeNull();
		result[0].Author.Id.Should().BeEquivalentTo(expectedUserId);
		result[1].Author.Id.Should().NotBeNull();
		result[1].Author.Id.Should().BeEquivalentTo(expectedUserId);

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Issues Waiting for Approval")]
	public async Task GetIssuesWaitingForApprovalAsync_ShouldReturnAListOfIssues_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		expected.ApprovedForRelease = false;
		expected.Archived = false;
		expected.Rejected = false;

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<IssueModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = (await sut.GetIssuesWaitingForApprovalAsync().ConfigureAwait(false)).First();

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.ApprovedForRelease.Should().BeFalse();

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
				It.IsAny<FindOptions<IssueModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Issues Approved")]
	public async Task GetIssuesApprovedAsync_Should_Return_AListOfApprovedIssues_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetIssues(1).First();
		expected.ApprovedForRelease = true;
		expected.Archived = false;
		expected.Rejected = false;

		//await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<IssueModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = (await sut.GetIssuesApprovedAsync().ConfigureAwait(false)).First();

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.ApprovedForRelease.Should().BeTrue();

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
				It.IsAny<FindOptions<IssueModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Issue")]
	public async Task UpdateIssueAsync_With_A_Valid_Id_And_Issue_Should_UpdateIssue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		expected.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedIssue = FakeIssue.GetNewIssue();
		updatedIssue.Id = expected.Id;
		updatedIssue.Archived = expected.Archived;
		updatedIssue.Author = expected.Author;
		updatedIssue.DateCreated = expected.DateCreated;
		updatedIssue.Description = "Updated Description";
		updatedIssue.Title = expected.Title;

		_list = new List<IssueModel> { updatedIssue };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<IssueModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateIssueAsync(updatedIssue);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<IssueModel>>(), updatedIssue,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);

	}

}
