namespace IssueTracker.Library.Services;

[ExcludeFromCodeCoverage]
public class IssueServiceTests
{
	private readonly Mock<IIssueRepository> _issueRepositoryMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<ICacheEntry> _mockCacheEntry;
	private IssueService _sut;

	public IssueServiceTests()
	{
		_issueRepositoryMock = new Mock<IIssueRepository>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_mockCacheEntry = new Mock<ICacheEntry>();
		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);
	}

	[Fact(DisplayName = "Create Issue With Valid Values")]
	public async Task CreateIssue_With_Valid_Values_Should_Return_Test()
	{
		// Arrange

		var issue = TestIssues.GetNewIssue();

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.CreateIssue(issue);

		// Assert

		_sut.Should().NotBeNull();

		_issueRepositoryMock
			.Verify(x =>
				x.CreateIssue(It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact(DisplayName = "Create Issue With Invalid Issue Throws Exception")]
	public async Task Create_With_Invalid_Issue_Should_Return_ArgumentNullException_TestAsync()
	{
		// Arrange

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateIssue(null));
	}

	[Fact(DisplayName = "Get Issue With Valid Id")]
	public async Task GetIssue_With_Valid_Id_Should_Return_Expected_Issue_Test()
	{
		//Arrange

		var expected = TestIssues.GetKnownIssue();

		_issueRepositoryMock.Setup(x => x.GetIssue(It.IsAny<string>())).ReturnsAsync(expected);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);


		//Act

		var result = await _sut.GetIssue(expected.Id);

		//Assert

		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
	}

	[Fact(DisplayName = "Get Issue With Empty String Id")]
	public async Task GetIssue_With_Empty_String_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetIssue(""));
	}

	[Fact(DisplayName = "Get Issue With Null Id")]
	public async Task GetIssue_With_Null_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetIssue(null));
	}

	[Fact(DisplayName = "Get Issues")]
	public async Task GetIssues_Should_Return_A_List_Of_Issues_Test()
	{
		//Arrange

		const int expectedCount = 3;

		var expected = TestIssues.GetIssues();

		_issueRepositoryMock.Setup(x => x.GetIssues()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetIssues();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Issues with cache")]
	public async Task GetIssues_With_Memory_Cache_Should_A_List_Of_Issues_Test()
	{
		//Arrange

		const int expectedCount = 3;

		var expected = TestIssues.GetIssues();


		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;
		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetIssues();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues With Valid Id")]
	public async Task GetUsersIssues_With_A_Valid_Id_Should_Return_A_List_Of_User_Issues_Test()
	{
		//Arrange

		const int expectedCount = 2;
		const string expectedUser = "5dc1039a1521eaa36835e541";

		var expected = TestIssues.GetIssuesWithDuplicateAuthors().Where(x => x.Author.Id == expectedUser).ToList();

		_issueRepositoryMock.Setup(x => x.GetUsersIssues(It.IsAny<string>())).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetUsersIssues(expectedUser);

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues with cache")]
	public async Task GetUsersIssues_With_Memory_Cache_Should_Return_A_List_Of_User_Issues_Test()
	{
		//Arrange

		const int expectedCount = 2;
		const string expectedUser = "5dc1039a1521eaa36835e541";

		var expected = TestIssues.GetIssuesWithDuplicateAuthors().Where(x => x.Author.Id == expectedUser).ToList();

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		object whatever = expected;
		_memoryCacheMock
			.Setup(mc => mc.TryGetValue(It.IsAny<object>(), out whatever))
			.Callback(new OutDelegate<object, object>((object _, out object v) =>
				v = whatever)) // mocked value here (and/or breakpoint)
			.Returns(true);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetUsersIssues(expectedUser);

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Get Users Issues With empty string")]
	public async Task GetUsersIssues_With_Empty_String_Users_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetUsersIssues(""));
	}

	[Fact(DisplayName = "Get Users Issues With Null Id")]
	public async Task GetUsersIssues_With_Null_Users_Id_Should_Return_An_ArgumentException_TestAsync()
	{
		// Arrange

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetUsersIssues(null));
	}

	[Fact(DisplayName = "GetIssuesWaitingForApproval")]
	public async Task GetIssuesWaitingForApproval_With_ValidData_Should_ReturnAListOfIssues_Test()
	{
		//Arrange

		const int expectedCount = 1;

		var expected = TestIssues.GetIssues().Where(c => c.ApprovedForRelease == false);

		_issueRepositoryMock.Setup(x => x.GetIssuesWaitingForApproval()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetIssuesWaitingForApproval().ConfigureAwait(false);

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "GetApprovedIssues")]
	public async Task GetApprovedIssues_With_ValidData_Should_ReturnAListOfIssues_Test()
	{
		//Arrange

		const int expectedCount = 2;

		var expected = TestIssues.GetIssues().Where(c => c.ApprovedForRelease);

		_issueRepositoryMock.Setup(x => x.GetApprovedIssues()).ReturnsAsync(expected);

		_memoryCacheMock
			.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
			.Callback((object k) => _ = (string)k)
			.Returns(_mockCacheEntry.Object);

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		//Act

		var results = await _sut.GetApprovedIssues();

		//Assert

		results.Should().NotBeNull();
		results.Count.Should().Be(expectedCount);
	}

	[Fact(DisplayName = "Update Issue With Valid Issue")]
	public async Task UpdateIssue_With_A_Valid_Issue_Should_Succeed_Test()
	{
		// Arrange

		var updatedIssue = TestIssues.GetUpdatedIssue();

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		await _sut.UpdateIssue(updatedIssue);

		// Assert

		_sut.Should().NotBeNull();

		_issueRepositoryMock
			.Verify(x =>
				x.UpdateIssue(It.IsAny<string>(), It.IsAny<IssueModel>()), Times.Once);
	}

	[Fact(DisplayName = "Update With Invalid Issue")]
	public async Task UpdateIssue_With_Invalid_Issue_Should_Return_ArgumentNullException_Test()
	{
		// Arrange

		_sut = new IssueService(_issueRepositoryMock.Object, _memoryCacheMock.Object);

		// Act

		// Assert

		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.UpdateIssue(null));
	}

	private delegate void OutDelegate<in TIn, TOut>(TIn input, out TOut output);
}