﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CommentRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Unit
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

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
		_cursor = TestFixtures.GetMockCursor(_list);
		_userCursor = TestFixtures.GetMockCursor(_users);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection(_userCursor);

		_mockContext = TestFixtures.GetMockContext();
	}

	private CommentRepository CreateRepository()
	{
		return new CommentRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Create Comment With Valid Comment")]
	public async Task CreateComment_With_A_Valid_Comment_Should_Return_Success_TestAsync()
	{
		// Arrange
		CommentModel newComment = FakeComment.GetNewComment();

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		UserModel user = FakeUser.GetNewUser(true);

		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		CommentRepository sut = CreateRepository();

		// Act
		await sut.CreateAsync(newComment);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(
				It.IsAny<CommentModel>(),
				null,
				default), Times.Once);
	}

	[Fact(DisplayName = "Get Comment With a Valid Id")]
	public async Task GetComment_With_Valid_Id_Should_Returns_One_Comment_TestAsync()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment(true);

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CommentRepository sut = CreateRepository();

		//Act
		CommentModel result = await sut.GetAsync(expected.Id).ConfigureAwait(false);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<CommentModel>>(),
				It.IsAny<FindOptions<CommentModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Comments")]
	public async Task GetComments_With_Valid_Context_Should_Return_A_List_Of_Comments_TestAsync()
	{
		// Arrange
		const int expectedCount = 5;
		List<CommentModel> expected = FakeComment.GetComments(expectedCount).ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_list = new List<CommentModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		CommentRepository sut = CreateRepository();

		// Act
		List<CommentModel> result = (await sut.GetAllAsync().ConfigureAwait(false))!.ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<CommentModel>>(),
				It.IsAny<FindOptions<CommentModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Users Comments with valid Id")]
	public async Task GetUsersComments_With_Valid_Users_Id_Should_Return_A_List_Of_Users_Comments_TestAsync()
	{
		// Arrange
		const int expectedCount = 1;

		CommentModel expected = FakeComment.GetNewComment(true);

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CommentRepository sut = CreateRepository();

		// Act
		List<CommentModel> results = (await sut.GetByUserAsync(expected.Author.Id).ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results[0].Author.Id.Should().NotBeNull();
		results[0].Author.DisplayName.Should().NotBeNull();

		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<CommentModel>>(),
				It.IsAny<FindOptions<CommentModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Update Comment")]
	public async Task UpdateComment_With_A_Valid_Id_And_Comment_Should_UpdateComment_TestAsync()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment(true);

		await _mockCollection.Object.InsertOneAsync(expected);

		CommentModel updatedComment = FakeComment.GetNewComment(true);
		updatedComment.Id = expected.Id;
		updatedComment.Archived = true;

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CommentRepository sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedComment.Id, updatedComment);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(
					It.IsAny<FilterDefinition<CommentModel>>(),
					updatedComment,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Archive Comment")]
	public async Task ArchiveComment_With_A_Valid_Id_And_Comment_Should_ArchiveComment_TestAsync()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment(true);

		await _mockCollection.Object.InsertOneAsync(expected);

		CommentModel updatedComment = FakeComment.GetNewComment(true);
		updatedComment.Id = expected.Id;
		updatedComment.Archived = true;

		_list = new List<CommentModel> { updatedComment };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CommentRepository sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedComment);

		// Assert
		_mockCollection.Verify(c => c
			.ReplaceOneAsync(
				It.IsAny<FilterDefinition<CommentModel>>(),
				updatedComment,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Up vote Comment With Valid Comment and User")]
	public async Task UpVoteComment_With_A_Valid_CommentId_And_UserId_Should_Return_Success_TestAsync()
	{
		// Arrange
		const int expectedCount = 1;
		CommentModel expected = FakeComment.GetNewComment(true);

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		UserModel user = FakeUser.GetNewUser(true);
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		CommentRepository sut = CreateRepository();

		// Act
		await sut.UpVoteAsync(expected.Id, user.Id);

		// Assert
		expected.UserVotes.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Up vote Comment With User Already Voted")]
	public async Task UpVoteComment_With_User_Already_Voted_Should_Remove_The_User_And_The_Comment_Test()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment(true);

		UserModel user = FakeUser.GetNewUser(true);
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		expected.UserVotes.Add(user.Id);

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		CommentRepository sut = CreateRepository();

		// Act
		await sut.UpVoteAsync(expected.Id, user.Id);

		// Assert
		expected.UserVotes.Count.Should().Be(0);
	}

	[Fact(DisplayName = "Get Comments By Issue")]
	public async Task GetCommentsByIssueAsync_With_ValidIssue_Should_Return_A_List_Of_Comments_TestAsync()
	{
		// Arrange
		CommentModel expected = FakeComment.GetComments(1).First();
		expected.Archived = false;

		_list = new List<CommentModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<CommentModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		CommentRepository sut = CreateRepository();

		//Act
		List<CommentModel> result = (await sut.GetByIssueAsync(expected.Issue!).ConfigureAwait(false)).ToList();

		//Assert 
		result.Should().NotBeNull();
		result.First().Should().BeEquivalentTo(expected);
		result.First().DateCreated.Should().NotBeBefore(Convert.ToDateTime("01/01/2000"));
		result.First().Issue.Should().NotBeNull();
		result.First().Issue.Should().BeEquivalentTo(expected.Issue);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<CommentModel>>(),
				It.IsAny<FindOptions<CommentModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}