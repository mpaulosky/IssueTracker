namespace IssueTracker.PlugIns.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class IssueRepositoryTests
{
	private readonly Mock<IAsyncCursor<IssueModel>> _cursor;
	private readonly Mock<IMongoCollection<IssueModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<IssueModel> _list = new();
	private IssueRepository _sut;

	public IssueRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = GetMockMongoContext();

		_sut = new IssueRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Archive Issue")]
	public async Task ArchiveIssue_With_A_Valid_Id_And_Issue_Should_ArchiveIssue_TestAsync()
	{
		// Arrange

		var expected = TestIssues.GetKnownIssue();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedIssue = TestIssues.GetKnownIssue();
		updatedIssue.Archived = true;

		_list = new List<IssueModel> { updatedIssue };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.ArchiveIssueAsync(updatedIssue);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<IssueModel>>(), updatedIssue,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Create Issue with valid Issue")]
	public async Task CreateIssue_With_Valid_Issue_Should_Insert_A_New_Issue_TestAsync()
	{

		// Arrange

		var newIssue = TestIssues.GetKnownIssue();

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.CreateIssueAsync(newIssue);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<IssueModel>(), null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Issue With a Valid Id")]
	public async Task GetIssue_With_Valid_Id_Should_Returns_One_Issue_TestAsync()
	{
		// Arrange

		var expected = TestIssues.GetKnownIssue();

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		//Act

		IssueModel result = await _sut.GetIssueAsync(expected!.Id!);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result!.Description!.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssues_With_Valid_Context_Should_Return_A_List_Of_Issues_Test()
	{
		// Arrange
		const int expectedCount = 6;
		_list = TestIssues.GetIssues().ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetIssuesAsync().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues")]
	public async Task GetUsersIssues_With_Valid_Id_Should_Return_A_List_Of_User_Issues_TestAsync()
	{
		// Arrange

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		var expected = TestIssues.GetIssuesWithDuplicateAuthors().ToList();

		_list = new List<IssueModel>(expected).Where(x => x!.Author!.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		IEnumerable<IssueModel> result = await _sut.GetIssuesByUserAsync(expectedUserId).ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(2);
	}

	[Fact(DisplayName = "GetIssues Waiting For Approval")]
	public async Task
		GetIssuesWaitingForApproval_With_ListOfIssues_Should_ReturnAListOfIssuesWaitingForApproval_Test()
	{
		// Arrange
		const int expectedCount = 3;
		_list = TestIssues.GetIssues().ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetIssuesWaitingForApprovalAsync().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(expectedCount);
	}

	[Fact(DisplayName = "Get Approved Issues")]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnAListOfIssues_Test()
	{
		// Arrange
		const int expectedCount = 2;
		_list = TestIssues.GetIssues().ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		IEnumerable<IssueModel> result = await _sut.GetApprovedIssuesAsync().ConfigureAwait(false);

		// Assert
		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<IssueModel>>(),
			It.IsAny<FindOptions<IssueModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(expectedCount);
	}

	[Fact(DisplayName = "Update Issue with valid Issue")]
	public async Task UpdateIssue_With_A_Valid_Id_And_Issue_Should_UpdateIssue_Test()
	{
		// Arrange

		var expected = TestIssues.GetKnownIssue();

		var updatedIssue = TestIssues.GetIssue(
			expected!.Id!,
			"Test Issue 1 updated",
			"A new test issue 1 updated",
			expected!.DateCreated!,
			expected!.Archived!,
			expected!.IssueStatus!,
			expected!.Category!);

		_list = new List<IssueModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<IssueModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.UpdateIssueAsync(updatedIssue.Id, updatedIssue);

		// Assert

		_mockCollection.Verify(
			c =>
				c.ReplaceOneAsync(
					It.IsAny<FilterDefinition<IssueModel>>(), updatedIssue,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

}
