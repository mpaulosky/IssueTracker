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

	[Fact(DisplayName = "Create Issue With Valid Values")]
	public async Task CreateIssue_With_Valid_Values_Should_Return_Test()
	{
		// Arrange
		var sut = UnitUnderTest();

		var issue = FakeIssue.GetNewIssue(true);

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
		var sut = UnitUnderTest();
		const string expectedParamName = "issue";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		var act = async () => { await sut.CreateIssue(null!); };

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
		var sut = UnitUnderTest();

		var expected = FakeIssue.GetNewIssue(true);

		_issueRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(expected);

		//Act
		var result = await sut.GetIssue(expected.Id);

		//Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
	}

	[Theory(DisplayName = "Get Issue With Invalid Id")]
	[InlineData(null, "issueId", "Value cannot be null.?*")]
	[InlineData("", "issueId", "The value cannot be an empty string.?*")]
	public async Task GetIssue_With_Invalid_Id_Should_Return_An_ArgumentException_TestAsync(string value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		var sut = UnitUnderTest();

		// Act
		var act = async () => { await sut.GetIssue(value); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssues_Should_Return_A_List_Of_Issues_Test()
	{
		//Arrange
		var sut = UnitUnderTest();

		const int expectedCount = 6;

		var expected = FakeIssue.GetIssues(expectedCount);

		_issueRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		var results = await sut.GetIssues();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Issues with cache")]
	public async Task GetIssues_With_Memory_Cache_Should_A_List_Of_Issues_Test()
	{
		//Arrange
		var sut = UnitUnderTest();

		const int expectedCount = 6;

		var expected = FakeIssue.GetIssues(expectedCount);

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
		var results = await sut.GetIssues();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues With Valid Id")]
	public async Task GetUsersIssues_With_A_Valid_Id_Should_Return_A_List_Of_User_Issues_Test()
	{
		//Arrange
		var sut = UnitUnderTest();

		const int expectedCount = 2;

		var issues = FakeIssue.GetIssues(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (var issue in issues)
		{
			issue.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		var expected = issues;

		_issueRepositoryMock.Setup(x => x.GetByUserAsync(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		//Act
		var results = await sut.GetIssuesByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues with cache")]
	public async Task GetUsersIssues_With_Memory_Cache_Should_Return_A_List_Of_User_Issues_Test()
	{
		//Arrange
		var sut = UnitUnderTest();
		const int expectedCount = 2;

		var issues = FakeIssue.GetIssues(expectedCount).ToList();

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		foreach (var issue in issues)
		{
			issue.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		var expected = issues;

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
		var results = await sut.GetIssuesByUser(expectedUserId);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Theory(DisplayName = "Get iIssues By User With Invalid Id")]
	[InlineData(null, "userId", "Value cannot be null.?*")]
	[InlineData("", "userId", "The value cannot be an empty string.?*")]
	public async Task GetUsersIssues_With_Empty_String_Users_Id_Should_Return_An_ArgumentException_TestAsync(string value,
		string expectedParamName, string expectedMessage)
	{
		// Arrange
		var sut = UnitUnderTest();

		// Act
		var act = async () => { await sut.GetIssuesByUser(value); };

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
		var sut = UnitUnderTest();

		const int expectedCount = 3;

		var expected = FakeIssue.GetIssues(expectedCount).ToList();

		foreach (var issue in expected)
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
		var results = await sut.GetIssuesWaitingForApproval().ConfigureAwait(false);

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "GetApprovedIssues")]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnAListOfIssues_Test()
	{
		//Arrange
		var sut = UnitUnderTest();

		const int expectedCount = 3;

		var expected = FakeIssue.GetIssues(expectedCount).ToList();
		foreach (var issue in expected)
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
		var results = await sut.GetApprovedIssues();

		//Assert
		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Update Issue With Valid Issue")]
	public async Task UpdateIssue_With_A_Valid_Issue_Should_Succeed_Test()
	{
		// Arrange
		var sut = UnitUnderTest();

		var updatedIssue = FakeIssue.GetNewIssue(true);

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
		var sut = UnitUnderTest();
		const string expectedParamName = "issue";
		const string expectedMessage = "Value cannot be null.?*";

		// Act
		var act = async () => { await sut.UpdateIssue(null!); };

		// Assert
		await act.Should()
			.ThrowAsync<ArgumentNullException>()
			.WithParameterName(expectedParamName)
			.WithMessage(expectedMessage);
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}
