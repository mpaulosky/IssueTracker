// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IssueServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services.Tests.Unit
// =============================================

namespace IssueTracker.Services.Issue;

[ExcludeFromCodeCoverage]
public class IssueServiceTests
{
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;

	public IssueServiceTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
	}

	private IssueService UnitUnderTest()
	{
		return new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Archive Issue With Invalid Issue Throws Exception")]
	public async Task ArchiveIssue_With_Invalid_Issue_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		IssueService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => await sut.ArchiveIssue(null!);

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName("issue")
			.WithMessage("Value cannot be null. (Parameter 'issue')");
	}

	[Fact(DisplayName = "Archive Issue With Valid Values")]
	public async Task ArchiveIssue_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel expected = FakeIssue.GetNewIssue(true);

		// Act
		await sut.ArchiveIssue(expected);

		// Assert
		sut.Should().NotBeNull();
		expected.Id.Should().Be(expected.Id);

		_issueRepositoryMock
			.Verify(x =>
				x.ArchiveAsync(It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Issue With Valid Values")]
	public async Task CreateIssue_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();

		IssueModel issue = FakeIssue.GetNewIssue(true);

		// Act
		await sut.CreateIssue(issue);

		// Assert
		sut.Should().NotBeNull();
		issue.Id.Should().NotBeNull();

		_issueRepositoryMock
			.Verify(x =>
				x.CreateAsync(It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Issue With Invalid Issue Throws Exception")]
	public async Task Create_With_Invalid_Issue_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		const string expectedParamName = "issue";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.CreateIssue(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Issue With Valid Id")]
	public async Task GetIssue_With_Valid_Id_Should_Return_Expected_Issue_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();

		IssueModel expected = FakeIssue.GetNewIssue(true);

		_issueRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		IssueModel? result = await sut.GetIssue(expected.Id, expected.Author.Id, false);

		//Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
	}

	[Theory(DisplayName = "Get Issue With Invalid Id")]
	[InlineData(null, "issueId", "Value cannot be null.?*")]
	[InlineData("", "issueId", "The value cannot be an empty string.?*")]
	public async Task GetIssue_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string? value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		IssueService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetIssue(value, "viewer-id", false); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Theory(DisplayName = "Get Issue Hides Pending And Rejected Issues From Other Users")]
	[InlineData(false, false)]
	[InlineData(false, true)]
	[InlineData(true, true)]
	public async Task GetIssue_With_Pending_Or_Rejected_Issue_And_Other_User_Should_Return_Null_Test(bool approved,
		bool rejected)
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);
		issue.ApprovedForRelease = approved;
		issue.Rejected = rejected;

		_issueRepositoryMock.Setup(x => x.GetAsync(issue.Id)).ReturnsAsync(issue);

		// Act
		IssueModel? result = await sut.GetIssue(issue.Id, "another-user-id", false);

		// Assert
		result.Should().BeNull();
	}

	[Theory(DisplayName = "Get Issue Shows Pending And Rejected Issues To Their Author And Admins")]
	[InlineData(false, false, true, false)]
	[InlineData(false, true, true, false)]
	[InlineData(false, false, false, true)]
	[InlineData(false, true, false, true)]
	public async Task GetIssue_With_Pending_Or_Rejected_Issue_And_Author_Or_Admin_Should_Return_Issue_Test(
		bool approved, bool rejected, bool viewerIsAuthor, bool viewerIsAdmin)
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);
		issue.ApprovedForRelease = approved;
		issue.Rejected = rejected;
		string viewerId = viewerIsAuthor ? issue.Author.Id : "another-user-id";

		_issueRepositoryMock.Setup(x => x.GetAsync(issue.Id)).ReturnsAsync(issue);

		// Act
		IssueModel? result = await sut.GetIssue(issue.Id, viewerId, viewerIsAdmin);

		// Assert
		result.Should().BeSameAs(issue);
	}

	[Fact(DisplayName = "Get Issue Shows Approved Issues To Every User")]
	public async Task GetIssue_With_Approved_Issue_And_Other_User_Should_Return_Issue_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);
		issue.ApprovedForRelease = true;
		issue.Rejected = false;

		_issueRepositoryMock.Setup(x => x.GetAsync(issue.Id)).ReturnsAsync(issue);

		// Act
		IssueModel? result = await sut.GetIssue(issue.Id, "another-user-id", false);

		// Assert
		result.Should().BeSameAs(issue);
	}

	[Fact(DisplayName = "Get Issue Does Not Match An Empty Viewer To An Empty Author")]
	public async Task GetIssue_With_Pending_Issue_And_Empty_Viewer_Id_Should_Return_Null_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);
		issue.ApprovedForRelease = false;
		issue.Rejected = false;
		issue.Author = new BasicUserModel();

		_issueRepositoryMock.Setup(x => x.GetAsync(issue.Id)).ReturnsAsync(issue);

		// Act
		IssueModel? result = await sut.GetIssue(issue.Id, string.Empty, false);

		// Assert
		result.Should().BeNull();
	}

	[Fact(DisplayName = "Get Issue With Unknown Id")]
	public async Task GetIssue_With_Unknown_Id_Should_Return_Null_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();

		_issueRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((IssueModel)null!);

		// Act
		IssueModel? result = await sut.GetIssue("5dc1039a1521eaa36835e541", "viewer-id", true);

		// Assert
		result.Should().BeNull();
	}

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssues_Should_Return_A_List_Of_Issues_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();

		const int expectedCount = 6;

		IEnumerable<IssueModel> expected = FakeIssue.GetIssues(expectedCount);

		_issueRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<IssueModel> results = await sut.GetIssues();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Issues with cache")]
	public async Task GetIssues_With_Memory_Cache_Should_A_List_Of_Issues_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();

		const int expectedCount = 6;

		IEnumerable<IssueModel> expected = FakeIssue.GetIssues(expectedCount);

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
		List<IssueModel> results = await sut.GetIssues();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues With Valid Id")]
	public async Task GetUsersIssues_With_A_Valid_Id_Should_Return_A_List_Of_User_Issues_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();

		const int expectedCount = 2;

		List<IssueModel> issues = FakeIssue.GetIssues(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (IssueModel? issue in issues)
		{
			issue.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		List<IssueModel> expected = issues;

		_issueRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<IssueModel> results = await sut.GetIssuesByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues with cache")]
	public async Task GetUsersIssues_With_Memory_Cache_Should_Return_A_List_Of_User_Issues_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();
		const int expectedCount = 2;

		List<IssueModel> issues = FakeIssue.GetIssues(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (IssueModel? issue in issues)
		{
			issue.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		List<IssueModel> expected = issues;

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
		List<IssueModel> results = await sut.GetIssuesByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Theory(DisplayName = "Get iIssues By User With Invalid Id")]
	[InlineData(null, "userId", "Value cannot be null.?*")]
	[InlineData("", "userId", "The value cannot be an empty string.?*")]
	public async Task GetUsersIssues_With_Empty_String_Users_Id_Should_Return_An_ArgumentException_TestAsync(string? value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		IssueService sut = UnitUnderTest();

		// Act
		Func<Task> act = async () => { await sut.GetIssuesByUser(value!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "GetIssuesWaitingForApproval")]
	public async Task GetIssuesWaitingForApproval_With_ValidData_Should_ReturnAListOfIssues_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();

		const int expectedCount = 3;

		List<IssueModel> expected = FakeIssue.GetIssues(expectedCount).ToList();

		foreach (IssueModel? issue in expected)
		{
			issue.ApprovedForRelease = false;
			issue.Archived = false;
			issue.Rejected = false;
		}

		_issueRepositoryMock.Setup(x => x.GetWaitingForApprovalAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<IssueModel> results = await sut.GetIssuesWaitingForApproval();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "GetApprovedIssues")]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnAListOfIssues_Test()
	{
		//Arrange
		IssueService sut = UnitUnderTest();

		const int expectedCount = 3;

		List<IssueModel> expected = FakeIssue.GetIssues(expectedCount).ToList();
		foreach (IssueModel? issue in expected)
		{
			issue.ApprovedForRelease = true;
			issue.Archived = false;
			issue.Rejected = false;
		}

		_issueRepositoryMock.Setup(x => x.GetApprovedAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		List<IssueModel> results = await sut.GetApprovedIssues();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Update Issue With Valid Issue")]
	public async Task UpdateIssue_With_A_Valid_Issue_Should_Succeed_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();

		IssueModel updatedIssue = FakeIssue.GetNewIssue(true);

		// Act
		await sut.UpdateIssue(updatedIssue);

		// Assert
		sut.Should().NotBeNull();

		_issueRepositoryMock
			.Verify(x =>
				x.UpdateAsync(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Issue")]
	public async Task UpdateIssue_With_Invalid_Issue_Should_Return_ArgumentNullException_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		const string expectedParamName = "issue";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		Func<Task> act = async () => { await sut.UpdateIssue(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);

	[Fact(DisplayName = "Create Issue Clears The Issue Caches")]
	public async Task CreateIssue_With_Valid_Issue_Should_Remove_Cached_Issues_And_Authors_Issues_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);

		// Act
		await sut.CreateIssue(issue);

		// Assert
		_memoryCacheMock.Verify(x => x.Remove("IssueData"), Times.Once);
		_memoryCacheMock.Verify(x => x.Remove(issue.Author.Id), Times.Once);
	}

	[Fact(DisplayName = "Update Issue Clears The Issue Caches")]
	public async Task UpdateIssue_With_Valid_Issue_Should_Remove_Cached_Issues_And_Authors_Issues_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);

		// Act
		await sut.UpdateIssue(issue);

		// Assert
		_memoryCacheMock.Verify(x => x.Remove("IssueData"), Times.Once);
		_memoryCacheMock.Verify(x => x.Remove(issue.Author.Id), Times.Once);
	}

	[Fact(DisplayName = "Archive Issue Clears The Issue Caches")]
	public async Task ArchiveIssue_With_Valid_Issue_Should_Remove_Cached_Issues_And_Authors_Issues_Test()
	{
		// Arrange
		IssueService sut = UnitUnderTest();
		IssueModel issue = FakeIssue.GetNewIssue(true);

		// Act
		await sut.ArchiveIssue(issue);

		// Assert
		_memoryCacheMock.Verify(x => x.Remove("IssueData"), Times.Once);
		_memoryCacheMock.Verify(x => x.Remove(issue.Author.Id), Times.Once);
	}
}
