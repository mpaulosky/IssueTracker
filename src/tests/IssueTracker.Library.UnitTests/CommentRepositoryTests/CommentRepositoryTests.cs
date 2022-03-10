using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IssueTracker.Library.UnitTests.CommentRepositoryTests;

[ExcludeFromCodeCoverage]
public class CommentRepositoryTests
{
	private CommentRepository _sut;
	private readonly Mock<IMongoCollection<Comment>> _mockCollection;
	private readonly Mock<IMongoCollection<User>> _mockUserCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Comment>> _cursor;
	private readonly Mock<IAsyncCursor<User>> _userCursor;
	private List<Comment> _list = new();
	private List<User> _users = new();
	
	public CommentRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor<Comment>(_list);
		_userCursor = TestFixtures.GetMockCursor<User>(_users);

		_mockCollection = TestFixtures.GetMockCollection<Comment>(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection<User>(_userCursor);

		_mockContext = TestFixtures.GetMockContext<Comment>();
	}

	[Fact(DisplayName = "Get Comment With a Valid Id")]
	public async Task GetComment_With_Valid_Id_Should_Returns_One_Comment_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		_list = new List<Comment> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetComment(expected.Id).ConfigureAwait(false);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Comment>>(),
			It.IsAny<FindOptions<Comment>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.DateCreated.Should().NotBeBefore(Convert.ToDateTime("01/01/2000"));
		result.UserVotes.Should().NotBeNull();
		result.Status.Should().NotBeNull();
	}

	[Fact(DisplayName = "Get Comment With Empty String Id")]
	public async Task GetComment_With_Empty_String_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange
		
		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => _sut.GetComment(""));
	}
	
	[Fact(DisplayName = "Get Comment With Null Id")]
	public async Task GetComment_With_Null_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange
		
		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);


		_sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetComment(null));
	}


	[Fact(DisplayName = "Get Comments")]
	public async Task GetComments_With_Valid_Context_Should_Return_A_List_Of_Comments_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetComments().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_list = new List<Comment>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetComments().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Comment>>(),
			It.IsAny<FindOptions<Comment>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
	}

	[Fact(DisplayName = "Get Users Comments with valid Id")]
	public async Task GetUsersComments_With_Valid_Users_Id_Should_Return_A_List_Of_Users_Comments_TestAsync()
	{
		// Arrange

		const string expectedUserId = "5dc1039a1521eaa36835e542";
		
		var expected = TestComments.GetComments().ToList();

		_list = new List<Comment>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);
		
		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetUsersComments(expectedUserId).ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Comment>>(),
			It.IsAny<FindOptions<Comment>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(1);
		items[0].Author.Id.Should().NotBeNull();
		items[0].Author.DisplayName.Should().NotBeNull();
	}

	[Fact(DisplayName = "Get Users Comments With empty string")]
	public async Task GetUsersComments_With_Empty_String_Users_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => _sut.GetUsersComments(""));
	}
	
	[Fact(DisplayName = "Get Users Comments With Null Id")]
	public async Task GetUsersComments_With_Null_Users_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetUsersComments(null));
	}
	
	[Fact(DisplayName = "Create Comment With No Users Throws Exception")]
	public async Task Create_With_Out_A_User_Should_Return_InvalidOperationException_TestAsync()
	{
		// Arrange
		
		var newComment = TestComments.GetKnownComment();
		await _mockUserCollection.Object.InsertOneAsync(TestUsers.GetKnownUser());
		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		_userCursor.Setup(_ => _.Current).Returns(_users);
		
		_sut = new CommentRepository(_mockContext.Object);
	
		// Act
	
		// Assert
	
		await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateComment(newComment));
	}
	
	[Fact(DisplayName = "Create Comment With Invalid Comment Throws Exception")]
	public async Task Create_With_Invalid_Comment_Should_Return_InvalidOperationException_TestAsync()
	{
		// Arrange
		
		await _mockUserCollection.Object.InsertOneAsync(TestUsers.GetKnownUser());
		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockUserCollection.Object);
		
		_users = new List<User>(){TestUsers.GetKnownUser()};

		_userCursor.Setup(_ => _.Current).Returns(_users);
		
		_sut = new CommentRepository(_mockContext.Object);
	
		// Act
	
		// Assert
	
		await Assert.ThrowsAsync<NullReferenceException>(() => _sut.CreateComment(null));
	}
		
	[Fact(DisplayName = "Create Comment With Valid Comment")]
	public async Task CreateComment_With_A_Valid_Comment_Should_Return_Success_TestAsync()
	{
		// Arrange
		
		var newComment = TestComments.GetKnownComment();

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockUserCollection.Object);
		
		_users = new List<User>(){TestUsers.GetKnownUser()};

		_userCursor.Setup(_ => _.Current).Returns(_users);
		
		_sut = new CommentRepository(_mockContext.Object);
	
		// Act
	
		await _sut.CreateComment(newComment);
	
		// Assert
	
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newComment, null, default(CancellationToken)), Times.Once);
	}
	
	[Fact(DisplayName = "Update Comment")]
	public async Task UpdateComment_With_A_Valid_Id_And_Comment_Should_UpdateComment_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		var updatedComment = TestComments.GetComment(expected.Id, "Test Comment Update", expected.Archived);

		_list = new List<Comment>(){updatedComment};

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		await _sut.UpdateComment(updatedComment.Id, updatedComment);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Comment>>(), updatedComment, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}


	[Fact(DisplayName = "Upvote Comment With Null Id")]
	public async Task UpvoteComment_With_Null_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange
		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetComment(null));
	}

	[Fact(DisplayName = "Upvote Comment With Valid Comment")]
	public async Task UpvoteComment_With_A_Valid_Comment_Should_Return_Success_TestAsync()
	{
		// Arrange
		
		var expected = TestComments.GetKnownComment();

		_list = new List<Comment> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<Comment>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		var user = TestUsers.GetKnownUser();
		_users = new List<User>(){user};

		_userCursor.Setup(_ => _.Current).Returns(_users);
		
		_sut = new CommentRepository(_mockContext.Object);
	
		// Act
	
		await _sut.UpvoteComment(expected.Id, user.Id);
	
		// Assert
	
		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Comment>>(), expected, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
		_mockUserCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<User>>(), user, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

}