using FluentAssertions;

using IssueTracker.Library.UnitTests.Fixtures;

using IssueTrackerLibrary.Contracts;
using IssueTrackerLibrary.DataAccess;
using IssueTrackerLibrary.Helpers;
using IssueTrackerLibrary.Models;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using Moq;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.Library.UnitTests.CommentRepositoryTests;

[ExcludeFromCodeCoverage]
public class CommentRepositoryTests
{
	private readonly IOptions<IssueTrackerDatabaseSettings> _options;
	private readonly Mock<IMongoCollection<Comment>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Comment>> _cursor;
	private List<Comment> _list = new();

	public CommentRepositoryTests()
	{
		_options = TestFixtures.Settings();

		_cursor = TestFixtures.MockCursor<Comment>();

		_mockCollection = TestFixtures.MockCollection<Comment>(_cursor);

		_mockContext = TestFixtures.MockContext<Comment>(_mockCollection);
	}

	[Fact(DisplayName = "Get Comment With a Valid Id")]
	public async Task Get_With_Valid_Id_Should_Returns_One_Comment_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Comment> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new CommentRepository(_mockContext.Object);

		//Act

		var result = await sut.Get(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Comment>>(),
			It.IsAny<FindOptions<Comment>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact(DisplayName = "Get Comment With Empty String Id")]
	public async Task Get_With_Empty_String_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => sut.Get(""));
	}
	
	[Fact(DisplayName = "Get Comment With Null Id")]
	public async Task Get_With_Null_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Get(null));
	}


	[Fact(DisplayName = "Get Comments")]
	public async Task Get_With_Valid_Context_Should_Return_A_List_Of_Comments_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetComments().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<Comment>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		var result = await sut.Get().ConfigureAwait(false);

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

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<Comment>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		var result = await sut.GetUsersComments(expectedUserId).ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Comment>>(),
			It.IsAny<FindOptions<Comment>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(2);
	}

	[Fact(DisplayName = "Get Users Comments With empty string")]
	public async Task GetUsersComments_With_Empty_String_Users_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => sut.GetUsersComments(""));
	}
	
	[Fact(DisplayName = "Get Users Comments With Null Id")]
	public async Task GetUsersComments_With_Null_Users_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => sut.GetUsersComments(null));
	}


	[Fact(DisplayName = "Create Comment")]
	public async Task Create_With_Valid_Comment_Should_Insert_A_New_Comment_TestAsync()
	{
		// Arrange

		var newComment = TestComments.GetKnownComment();
		var sut = new CommentRepository(_mockContext.Object);

		// Act

		await sut.Create(newComment);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newComment, null, default(CancellationToken)), Times.Once);
	}

	[Fact(DisplayName = "Update Comment")]
	public async Task Update_With_A_Valid_Id_And_Comment_Should_UpdateComment_TestAsync()
	{
		// Arrange

		var expected = TestComments.GetKnownComment();

		var updatedComment = TestComments.GetComment(expected.Id, "Test Comment Update", expected.Archived);

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Comment>(){updatedComment};

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new CommentRepository(_mockContext.Object);

		// Act

		await sut.Update(updatedComment.Id, updatedComment);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Comment>>(), updatedComment, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}