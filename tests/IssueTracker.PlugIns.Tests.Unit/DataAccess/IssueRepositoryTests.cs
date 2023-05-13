
namespace IssueTracker.PlugIns.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class IssueRepositoryTests
{

	private readonly Mock<IAsyncCursor<IssueModel>> _cursor;
	private readonly Mock<IMongoCollection<IssueModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<IssueModel> _list = new();

	public IssueRepositoryTests()
	{

		_cursor = TestFixturesMongo.GetMockCursor(_list);

		_mockCollection = TestFixturesMongo.GetMockCollection(_cursor);

		_mockContext = GetMockMongoContext();

	}

	private IssueRepository CreateRepository()
	{

		return new IssueRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Archive Issue")]
	public async Task ArchiveIssue_With_A_Valid_Id_And_Issue_Should_ArchiveIssue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue(true);

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedIssue = FakeIssue.GetNewIssue(true);
		updatedIssue.Archived = true;

		_list = new List<IssueModel> { updatedIssue };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedIssue);

		// Assert
		_mockCollection.Verify(
			c => c
			.ReplaceOneAsync(
				It.IsAny<FilterDefinition<IssueModel>>(),
				updatedIssue,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Create Issue with valid Issue")]
	public async Task CreateIssue_With_Valid_Issue_Should_Insert_A_New_Issue_TestAsync()
	{

		// Arrange
		var newIssue = FakeIssue.GetNewIssue(true);

		_mockContext.Setup(c => c
		.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newIssue);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
		.InsertOneAsync(
			It.IsAny<IssueModel>(),
			null,
			default), Times.Once);

	}

	[Fact(DisplayName = "Get Issue With a Valid Id")]
	public async Task GetIssue_With_Valid_Id_Should_Returns_One_Issue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue(true);

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		IssueModel result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.Description.Length.Should().BeGreaterThan(1);


		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssues_With_Valid_Context_Should_Return_A_List_Of_Issues_Test()
	{

		// Arrange
		const int expectedCount = 6;
		_list = FakeIssue.GetIssues(expectedCount).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users Issues")]
	public async Task GetUsersIssues_With_Valid_Id_Should_Return_A_List_Of_User_Issues_TestAsync()
	{

		// Arrange
		const int expectedCount = 2;
		const string expectedUserId = "5dc1039a1521eaa36835e541";

		var expected = FakeIssue.GetIssues(expectedCount).ToList();

		foreach (var item in expected)
		{
			item.Author = new BasicUserModel(expectedUserId, "test");
		}

		_list = new List<IssueModel>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		IEnumerable<IssueModel> results = (await sut.GetByUserAsync(expectedUserId).ConfigureAwait(false)).ToList();

		// Assert
		var items = results.ToList();
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "GetIssues Waiting For Approval")]
	public async Task
		GetIssuesWaitingForApproval_With_ListOfIssues_Should_ReturnAListOfIssuesWaitingForApproval_Test()
	{

		// Arrange
		const int expectedCount = 3;
		_list = FakeIssue.GetIssues(expectedCount).ToList();

		foreach (var item in _list)
		{

			item.ApprovedForRelease = false;
			item.Archived = false;
			item.Rejected = false;

		}

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetWaitingForApprovalAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);


		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Approved Issues")]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnAListOfIssues_Test()
	{

		// Arrange
		const int expectedCount = 2;
		_list = FakeIssue.GetIssues(expectedCount).ToList();

		foreach (var item in _list)
		{
			item.ApprovedForRelease = true;
			item.Archived = false;
			item.Rejected = false;
		}

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		IEnumerable<IssueModel> results = (await sut.GetApprovedAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
		.FindAsync(
			It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Issue with valid Issue")]
	public async Task UpdateIssue_With_A_Valid_Id_And_Issue_Should_UpdateIssue_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue(true);

		IssueModel updatedIssue = new()
		{
			Id = expected.Id,
			Title = "Test Issue 1 updated",
			Description = "A new test issue 1 updated",
			DateCreated = expected.DateCreated,
			Archived = expected.Archived,
			IssueStatus = expected.IssueStatus,
			Category = expected.Category
		};

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedIssue.Id, updatedIssue);

		// Assert
		_mockCollection.Verify(
			c => c
			.ReplaceOneAsync(
				It.IsAny<FilterDefinition<IssueModel>>(),
				updatedIssue,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

}
