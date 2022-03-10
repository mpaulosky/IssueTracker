﻿using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace IssueTracker.Library.UnitTests.IssueRepositoryTests;

[ExcludeFromCodeCoverage]
public class IssueRepositoryTests
{
	private IssueRepository _sut;
	private readonly Mock<IMongoCollection<Issue>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<Issue>> _cursor;
	private List<Issue> _list = new();


	public IssueRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor<Issue>(_list);

		_mockCollection = TestFixtures.GetMockCollection<Issue>(_cursor);

		_mockContext = TestFixtures.GetMockContext<Issue>();
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

		var result = await _sut.Get(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if InsertOneAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<Issue>>(),
			It.IsAny<FindOptions<Issue>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.Description.Length.Should().BeGreaterThan(1);
	}
	
	[Fact(DisplayName = "Get Issue With Empty String Id")]
	public async Task Get_With_Empty_String_Id_Should_Return_An_IndexOutOfRangeException_TestAsync()
	{
		// Arrange

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<IndexOutOfRangeException>(() => _sut.Get(""));
	}
	
	[Fact(DisplayName = "Get Issue With Null Id")]
	public async Task Get_With_Null_Id_Should_Return_A_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Get(null));
	}
	
	[Fact(DisplayName = "Get Issues")]
	public async Task Get_With_Valid_Context_Should_Return_A_List_Of_Issues_Test()
	{
		// Arrange

		_list = TestIssues.GetIssues().ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);
		
		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		var result = await _sut.Get().ConfigureAwait(false);

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
		
		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.Create(newIssue);

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

		_list = new List<Issue> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);
		
		_mockContext.Setup(c => c.GetCollection<Issue>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var _sut = new IssueRepository(_mockContext.Object);

		// Act

		await _sut.Update(updatedIssue.Id, updatedIssue);

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
}