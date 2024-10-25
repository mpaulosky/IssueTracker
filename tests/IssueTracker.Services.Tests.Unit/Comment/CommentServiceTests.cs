﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CommentServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services.Tests.Unit
// =============================================

namespace IssueTracker.Services.Comment;

[ExcludeFromCodeCoverage]
public class CommentServiceTests
{
	private readonly Mock<ICommentRepository> _commentRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public CommentServiceTests()
	{
		_commentRepositoryMock = new Mock<ICommentRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	private CommentService UnitUnderTest()
	{
		return new CommentService(_commentRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Archive Comment With Invalid Comment Throws Exception")]
	public async Task ArchiveComment_With_Invalid_Comment_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => await sut.ArchiveComment(null!);

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName("comment")
			.WithMessage("Value cannot be null. (Parameter 'comment')");
	}

	[Fact(DisplayName = "Archive Comment With Valid Values")]
	public async Task ArchiveComment_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		CommentService sut = UnitUnderTest();
		CommentModel expected = FakeComment.GetNewComment(true);

		// Act
		await sut.ArchiveComment(expected);

		// Assert
		sut.Should().NotBeNull();
		expected.Id.Should().Be(expected.Id);

		_commentRepositoryMock
			.Verify(x =>
				x.ArchiveAsync(It.IsAny<CommentModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Comment With Valid Values")]
	public async Task CreateComment_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		CommentModel comment = FakeComment.GetNewComment();

		// Act
		await sut.CreateComment(comment);

		// Assert
		sut.Should().NotBeNull();
		comment.Id.Should().NotBeNull();

		_commentRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<CommentModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Comment With Invalid Comment Throws Exception")]
	public async Task Create_With_Invalid_Comment_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		CommentService sut = UnitUnderTest();
		const string expectedParamName = "comment";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.CreateComment(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Comment With Valid Id")]
	public async Task GetComment_With_Valid_Id_Should_Return_Expected_Comment_Test()
	{
		//Arrange
		CommentService sut = UnitUnderTest();

		CommentModel expected = FakeComment.GetNewComment(true);

		_commentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		CommentModel result = await sut.GetComment(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Theory(DisplayName = "Get Comment With Invalid Id")]
	[InlineData(null, "commentId", "Value cannot be null.?*")]
	[InlineData("", "commentId", "The value cannot be an empty string.?*")]
	public async Task GetComment_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetComment(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Comments")]
	public async Task GetComments_Should_Return_A_List_Of_Comments_Test()
	{
		//Arrange
		CommentService sut = UnitUnderTest();

		const int expectedCount = 3;

		List<CommentModel> expected = FakeComment.GetComments(expectedCount).ToList();

		foreach (CommentModel? comment in expected)
		{
			comment.Archived = false;
		}

		_commentRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<CommentModel> results = await sut.GetComments();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Comments with cache")]
	public async Task GetComments_With_Memory_Cache_Should_A_List_Of_Comments_Test()
	{
		//Arrange
		CommentService sut = UnitUnderTest();

		const int expectedCount = 3;

		IEnumerable<CommentModel> expected = FakeComment.GetComments(expectedCount);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;

		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever!))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		//Act
		List<CommentModel> results = await sut.GetComments();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Comments With Valid Id")]
	public async Task GetByUserAsync_With_A_Valid_Id_Should_Return_A_List_Of_User_Comments_Test()
	{
		//Arrange
		CommentService sut = UnitUnderTest();

		const int expectedCount = 2;
		List<CommentModel> comments = FakeComment.GetComments(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (CommentModel? comment in comments)
		{
			comment.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		List<CommentModel> expected = comments.ToList();

		_commentRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<CommentModel> results = await sut.GetCommentsByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Comments with memory cache")]
	public async Task GetByUserAsync_With_Cache_Should_Return_A_ListOfComments_TestAsync()
	{
		//Arrange
		CommentService sut = UnitUnderTest();

		const int expectedCount = 3;
		List<CommentModel> comments = FakeComment.GetComments(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (CommentModel? comment in comments)
		{
			comment.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		List<CommentModel> expected = comments.ToList();

		_commentRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<CommentModel> results = await sut.GetCommentsByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Theory(DisplayName = "Get Comments By User With Invalid Id")]
	[InlineData(null, "userId", "Value cannot be null.?*")]
	[InlineData("", "userId", "The value cannot be an empty string.?*")]
	public async Task GetUsersComments_With_Empty_String_Users_Id_Should_Return_An_ArgumentException_TestAsync(
		string value, string expectedParamName, string expectedMessage)
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetCommentsByUser(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Update Comment With Valid Comment")]
	public async Task UpdateComment_With_A_Valid_Comment_Should_Succeed_Test()
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		CommentModel updatedComment = FakeComment.GetNewComment(true);

		// Act
		await sut.UpdateComment(updatedComment);

		// Assert
		sut.Should().NotBeNull();

		_commentRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Comment")]
	public async Task UpdateComment_With_Invalid_Comment_Should_Return_ArgumentNullException_Test()
	{
		// Arrange
		CommentService sut = UnitUnderTest();
		const string expectedParamName = "comment";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.UpdateComment(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Up Vote Comment")]
	public async Task UpVoteComment_With_Valid_Inputs_Should_Be_Successful_Test()
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		const string testId = "5dc1039a1521eaa36835e543";

		CommentModel comment = FakeComment.GetNewComment(true);

		// Act
		await sut.UpVoteComment(comment.Id, testId);

		// Assert
		_commentRepositoryMock
			.Verify(x =>
				x.UpVoteAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
	}

	[Theory(DisplayName = "UpVote Comment With Invalid inputs")]
	[InlineData(null, "1", "commentId", "Value cannot be null.?*")]
	[InlineData("1", null, "userId", "Value cannot be null.?*")]
	public async Task UpVoteComment_With_Invalid_Inputs_Should_Return_An_ArgumentNullException_TestAsync(
		string commentId,
		string userId,
		string expectedParamName,
		string expectedMessage)
	{
		// Arrange
		CommentService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.UpVoteComment(commentId, userId); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}