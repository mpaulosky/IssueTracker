
using MongoDB.Driver;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IssueTracker.Library.UnitTests.DataAccess;

[ExcludeFromCodeCoverage]
public class IssueRepositoryTests
{
	private IssueRepository _sut;
	private readonly Mock<IMongoCollection<Issue>> _mockCollection;
	private readonly Mock<IMongoCollection<User>> _mockUserCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Issue>> _cursor;
	private readonly Mock<IAsyncCursor<User>> _userCursor;
	private List<Issue> _list = new();
	private List<User> _users = new();

	public IssueRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);
		_userCursor = TestFixtures.GetMockCursor(_users);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection(_userCursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new IssueRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Create Issue with valid Issue")]
	public async Task CreateIssue_With_Valid_Issue_Should_Insert_A_New_Issue_TestAsync()
	{
		// Arrange

		var newIssue = TestIssues.GetKnownIssue();

		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		var user = TestUsers.GetKnownUser();
		_users = new List<User> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.CreateIssue(newIssue).ConfigureAwait(false);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c =>
			c.InsertOneAsync(newIssue, null, default), Times.Once);
		_mockUserCollection.Verify(c =>
				c.ReplaceOneAsync(It.IsAny<FilterDefinition<User>>(), user, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Issue With a Valid Id")]
	public async Task GetIssue_With_Valid_Id_Should_Returns_One_Issue_TestAsync()
	{
		// Arrange

		var expected = TestIssues.GetKnownIssue();

		_list = new List<Issue> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetIssue(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.Description.Length.Should().BeGreaterThan(1);
	}

	// TODO: Move to IssueService Tests
	// [Fact(DisplayName = "Get Issue With Empty String Id")]
	// public async Task GetIssue_With_Empty_String_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	// {
	// 	// Arrange
	//
	// 	_sut = new IssueRepository(_mockContext.Object);
	//
	// 	// Act
	//
	// 	// Assert
	//
	// 	await Assert.ThrowsAsync<IndexOutOfRangeException>(() => _sut.GetIssue(""));
	// }

	// TODO: Move to IssueService Tests
	// [Fact(DisplayName = "Get Issue With Null Id")]
	// public async Task GetIssue_With_Null_Id_Should_Return_A_ArgumentNullException_TestAsync()
	// {
	// 	// Arrange
	//
	// 	_sut = new IssueRepository(_mockContext.Object);
	//
	// 	// Act
	//
	// 	// Assert
	//
	// 	await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetIssue(null));
	// }

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssues_With_Valid_Context_Should_Return_A_List_Of_Issues_Test()
	{
		// Arrange

		_list = TestIssues.GetIssues().ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetIssues().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
	}

	[Fact(DisplayName = "Get Users Issues")]
	public async Task GetUsersIssues_With_Valid_Id_Should_Return_A_List_Of_User_Issues_TestAsync()
	{
		// Arrange

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		var expected = TestIssues.GetIssuesWithDuplicateAuthors().ToList();

		_list = new List<Issue>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetUsersIssues(expectedUserId).ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(2);
	}

	[Fact(DisplayName = "Update Issue with valid Issue")]
	public async Task UpdateIssue_With_A_Valid_Id_And_Issue_Should_UpdateIssue_Test()
	{
		// Arrange

		var expected = TestIssues.GetKnownIssue();

		var updatedIssue = TestIssues.GetIssue(
			expected.Id,
			"Test Issue 1 updated",
			"A new test issue 1 updated",
			expected.DateCreated,
			expected.Archived,
			expected.IssueStatus,
			expected.OwnerNotes);

		_list = new List<Issue> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.UpdateIssue(updatedIssue.Id, updatedIssue);

		// Assert

		_mockCollection.Verify(
			c =>
				c.ReplaceOneAsync(
					It.IsAny<FilterDefinition<Issue>>(), updatedIssue,
					It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}