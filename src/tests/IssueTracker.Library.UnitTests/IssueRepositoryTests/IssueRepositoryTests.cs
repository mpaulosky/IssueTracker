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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.Library.UnitTests.IssueRepositoryTests;

[ExcludeFromCodeCoverage]
public class IssueRepositoryTests
{
	private readonly IOptions<IssueTrackerDatabaseSettings> _options;
	private readonly Mock<IMongoCollection<Issue>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Issue>> _cursor;
	private List<Issue> _list = new();

	public IssueRepositoryTests()
	{
		_options = TestFixtures.Settings();

		_cursor = TestFixtures.MockCursor<Issue>();

		_mockCollection = TestFixtures.MockCollection<Issue>(_cursor);

		_mockContext = TestFixtures.MockContext<Issue>(_mockCollection);
	}

	[Fact(DisplayName = "Get Issue With a Valid Id")]
	public async Task GetIssue_With_Valid_Id_Should_Returns_One_Issue_TestAsync()
	{
		// Arrange
		
		var expected = TestIssues.GetKnownIssue();

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Issue> { expected };
		
		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new IssueRepository(_mockContext.Object);

		//Act

		var result = await sut.Get(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}
	
	[Fact(DisplayName = "Get Issue With Empty String Id")]
	public async Task Get_With_Empty_String_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		var sut = new IssueRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => sut.Get(""));
	}
	
	[Fact(DisplayName = "Get Issue With Null Id")]
	public async Task Get_With_Null_Id_Should_Return_A_ArgumentNullException_TestAsync()
	{
		// Arrange

		var sut = new IssueRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Get(null));
	}
	
	[Fact(DisplayName = "Get Issues")]
	public async Task Get_With_Valid_Context_Should_Return_A_List_Of_Issues_Test()
	{
		// Arrange

		var expected = TestIssues.GetIssues().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<Issue>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await sut.Get().ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
	}

	[Fact(DisplayName = "Create Issue")]
	public async Task Create_With_Valid_Issue_Should_Insert_A_New_Issue_TestAsync()
	{
		// Arrange

		var newIssue = TestIssues.GetKnownIssue();
		var sut = new IssueRepository(_mockContext.Object);

		// Act

		await sut.Create(newIssue);

		// Assert

		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c.InsertOneAsync(newIssue, null, default(CancellationToken)), Times.Once);
	}

	[Fact(DisplayName = "Update Issue")]
	public async Task Update_With_A_Valid_Id_And_Issue_Should_UpdateIssue_Test()
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

		await _mockCollection.Object.InsertOneAsync(expected);

		_list = new List<Issue>();

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new IssueRepository(_mockContext.Object);

		// Act

		await sut.Update(updatedIssue.Id, updatedIssue);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Issue>>(), updatedIssue, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Users Issues")]
	public async Task GetUsersIssues_With_Valid_Id_Should_Return_A_List_Of_User_Issues_TestAsync()
	{
		// Arrange

		const string expectedUserId = "5dc1039a1521eaa36835e541";
		var expected = TestIssues.GetIssuesWithDuplicateAuthors().ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_list = new List<Issue>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await sut.GetUsersIssues(expectedUserId).ConfigureAwait(false);

		// Assert

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(2);	
	}
}