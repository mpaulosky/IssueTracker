// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     SolutionRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Unit
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
public class SolutionRepositoryTests
{
	private readonly Mock<IAsyncCursor<SolutionModel>> _cursor;
	private readonly Mock<IMongoCollection<SolutionModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private readonly Mock<IMongoCollection<UserModel>> _mockUserCollection;
	private readonly Mock<IAsyncCursor<UserModel>> _userCursor;
	private List<SolutionModel> _list = new();
	private List<UserModel> _users = new();

	public SolutionRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);
		_userCursor = TestFixtures.GetMockCursor(_users);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);
		_mockUserCollection = TestFixtures.GetMockCollection(_userCursor);

		_mockContext = TestFixtures.GetMockContext();
	}

	private SolutionRepository SystemUnderTest()
	{
		return new SolutionRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Archive Solution")]
	public async Task ArchiveAsync_With_A_Valid_Id_And_Solution_Should_ArchiveSolution_TestAsync()
	{
		// Arrange
		SolutionModel expected = FakeSolution.GetNewSolution(true);

		await _mockCollection.Object.InsertOneAsync(expected);

		SolutionModel expectedSolution = FakeSolution.GetNewSolution(true);

		_list = new List<SolutionModel> { expectedSolution };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		await sut.ArchiveAsync(expectedSolution);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(
					It.IsAny<FilterDefinition<SolutionModel>>(),
					expectedSolution,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Create Solution with valid Solution")]
	public async Task CreateAsync_With_Valid_Solution_Should_Insert_A_New_Solution_TestAsync()
	{
		// Arrange
		SolutionModel newSolution = FakeSolution.GetNewSolution(true);

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		await sut.CreateAsync(newSolution);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(
				It.IsAny<SolutionModel>(),
				null,
				default), Times.Once);
	}

	[Fact(DisplayName = "Get Solution With a Valid Id")]
	public async Task GetAsync_With_Valid_Id_Should_Returns_One_Solution_TestAsync()
	{
		// Arrange
		SolutionModel expected = FakeSolution.GetNewSolution(true);

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		//Act
		SolutionModel result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);
		result.Description.Length.Should().BeGreaterThan(1);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<SolutionModel>>(),
				It.IsAny<FindOptions<SolutionModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Solutions")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Solutions_Test()
	{
		// Arrange
		const int expectedCount = 6;
		_list = FakeSolution.GetSolutions(expectedCount).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		List<SolutionModel> results = (await sut.GetAllAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<SolutionModel>>(),
				It.IsAny<FindOptions<SolutionModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Solutions by User")]
	public async Task GetByUserAsync_With_Valid_Id_Should_Return_A_List_Of_User_Solutions_TestAsync()
	{
		// Arrange
		const int expectedCount = 3;

		const string expectedUserId = "5dc1039a1521eaa36835e541";

		List<SolutionModel> expected = FakeSolution.GetSolutions(expectedCount).ToList();

		foreach (SolutionModel? item in expected)
		{
			item.Author = new BasicUserModel(expectedUserId, "Jim", "Jones", "jimjones@test.com", "jimjones");
		}

		_list = new List<SolutionModel>(expected).Where(x => x.Author.Id == expectedUserId).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		IEnumerable<SolutionModel> results = (await sut.GetByUserAsync(expectedUserId).ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<SolutionModel>>(),
				It.IsAny<FindOptions<SolutionModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Solutions by Issue")]
	public async Task GetByIssueAsync_With_Valid_Id_Should_Return_A_List_Of_Issue_Solutions_TestAsync()
	{
		// Arrange
		const int expectedCount = 3;

		List<SolutionModel> expected = FakeSolution.GetSolutions(expectedCount).ToList();

		IssueModel expectedIssue = FakeIssue.GetNewIssue(true);

		foreach (SolutionModel? item in expected)
		{
			item.Issue = new BasicIssueModel(expectedIssue);
		}

		_list = new List<SolutionModel>(expected).Where(x => x.Issue.Id == expectedIssue.Id).ToList();

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		IEnumerable<SolutionModel> results = (await sut.GetByIssueAsync(expectedIssue.Id).ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<SolutionModel>>(),
				It.IsAny<FindOptions<SolutionModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Up vote Solution With Valid Solution and User")]
	public async Task UpVoteSolution_With_A_Valid_SolutionId_And_UserId_Should_Return_Success_TestAsync()
	{
		// Arrange
		const int expectedCount = 1;
		SolutionModel expected = FakeSolution.GetNewSolution(true);

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		UserModel user = FakeUser.GetNewUser(true);
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		_mockContext.Setup(c => c.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		await sut.UpVoteAsync(expected.Id, user.Id);

		// Assert
		expected.UserVotes.Count.Should().Be(expectedCount);
	}


	[Fact(DisplayName = "Update Solution with valid Solution")]
	public async Task UpdateAsync_With_A_Valid_Id_And_Solution_Should_UpdateSolution_Test()
	{
		// Arrange
		SolutionModel expected = FakeSolution.GetNewSolution(true);

		SolutionModel updatedSolution = new()
		{
			Id = expected.Id,
			Title = "Test Solution 1 updated",
			Description = "A new test solution 1 updated",
			DateCreated = expected.DateCreated,
			Archived = expected.Archived
		};

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		await sut.UpdateAsync(updatedSolution.Id, updatedSolution);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(
					It.IsAny<FilterDefinition<SolutionModel>>(),
					updatedSolution,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Up vote Solution With User Already Voted")]
	public async Task UpVoteSolution_With_User_Already_Voted_Should_Remove_The_User_And_The_Solution_Test()
	{
		// Arrange
		SolutionModel expected = FakeSolution.GetNewSolution(true);

		UserModel user = FakeUser.GetNewUser(true);
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		expected.UserVotes.Add(user.Id);

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		SolutionRepository sut = SystemUnderTest();

		// Act
		await sut.UpVoteAsync(expected.Id, user.Id);

		// Assert
		expected.UserVotes.Count.Should().Be(0);
	}
}