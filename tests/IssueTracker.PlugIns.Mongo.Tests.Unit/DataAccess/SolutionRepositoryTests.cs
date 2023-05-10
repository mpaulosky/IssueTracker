namespace IssueTracker.PlugIns.Mongo.Tests.Unit.DataAccess;

[ExcludeFromCodeCoverage]
public class SolutionRepositoryTests
{

	private readonly Mock<IAsyncCursor<SolutionModel>> _cursor;
	private readonly Mock<IAsyncCursor<UserModel>> _userCursor;
	private readonly Mock<IMongoCollection<SolutionModel>> _mockCollection;
	private readonly Mock<IMongoCollection<UserModel>> _mockUserCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
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

	private SolutionRepository CreateRepository()
	{

		return new SolutionRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Create Solution Test")]
	public async Task CreateSolutionAsync_With_Valid_Solution_Should_Insert_A_New_Solution_TestAsync()
	{

		// Arrange
		var newSolution = FakeSolution.GetNewSolution();

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newSolution);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(newSolution, null, default), Times.Once);

	}

	[Fact(DisplayName = "Get Solution By Id")]
	public async Task GetSolutionByIdAsync_With_Valid_Id_Should_Returns_One_Solution_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);


		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<SolutionModel>>(),
			It.IsAny<FindOptions<SolutionModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Solution By Issue Id")]
	public async Task GetSolutionsByIssueIdAsync_With_Valid_Issue_Id_Should_Returns_A_List_Of_Solutions_TestAsync()
	{

		// Arrange
		const int expectedCount = 1;
		var expected = FakeSolution.GetSolutions(expectedCount).First();
		expected.Archived = false;

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
					.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var results = (await sut.GetByIssueAsync(expected.Issue.Id))!.First();

		//Assert 
		results.Should().NotBeNull();
		results.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
					.FindAsync(It.IsAny<FilterDefinition<SolutionModel>>(),
									It.IsAny<FindOptions<SolutionModel>>(),
												It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Solutions By User Id")]
	public async Task GetSolutionByUserIdAsync_With_Valid_User_Id_Should_Return_A_List_Of_Solutions_TestAsync()
	{

		// Arrange
		const int expectedCount = 1;
		var expected = FakeSolution.GetSolutions(expectedCount).First();
		expected.Archived = false;

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var results = (await sut.GetByUserAsync(expected.Author.Id))!.First();

		//Assert 
		results.Should().NotBeNull();
		results.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<SolutionModel>>(),
				It.IsAny<FindOptions<SolutionModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Solutions Test")]
	public async Task GetSolutionsAsync_With_Valid_Context_Should_Return_A_List_Of_Solutions_Test()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeSolution.GetSolutions(expectedCount).ToList();

		_list = new List<SolutionModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync())!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<SolutionModel>>(),
			It.IsAny<FindOptions<SolutionModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Solution Test")]
	public async Task UpdateSolutionAsync_With_A_Valid_Id_And_Solution_Should_UpdateSolution_Test()
	{

		// Arrange
		var expected = FakeSolution.GetNewSolution();
		expected.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedSolution = FakeSolution.GetNewSolution();

		updatedSolution.Id = expected.Id;
		updatedSolution.Title = expected.Title;
		updatedSolution.Description = "Updated New SolutionDescription";
		updatedSolution.Issue = expected.Issue;
		updatedSolution.Author = expected.Author;
		updatedSolution.Archived = expected.Archived;
		updatedSolution.UserVotes = expected.UserVotes;

		_mockContext.Setup(c => c
			.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedSolution).ConfigureAwait(false);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<SolutionModel>>(), updatedSolution,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Up vote Solution With Valid Solution and User")]
	public async Task UpVoteAsync_With_A_Valid_SolutionId_And_UserId_Should_Return_Success_TestAsync()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<SolutionModel>(It.IsAny<string>())).Returns(_mockCollection.Object);
		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockUserCollection.Object);

		var user = FakeUser.GetNewUser(true);
		_users = new List<UserModel> { user };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		var sut = CreateRepository();

		// Act
		await sut.UpVoteAsync(expected.Id, user.Id);

		// Assert
		expected.UserVotes.Count.Should().BeGreaterThan(0);

	}

	[Fact(DisplayName = "Up vote Solution With User Already Voted")]
	public async Task UpVoteSolution_With_User_Already_Voted_Should_Remove_The_User_And_The_Solution_Test()
	{

		// Arrange
		var expected = FakeSolution.GetSolutions(1).First();
		var expectedUser = FakeUser.GetUsers(1).First();
		expected.UserVotes.Add(expectedUser.Id);

		_list = new List<SolutionModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<SolutionModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_mockContext.Setup(c => c
				.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockUserCollection.Object);

		_users = new List<UserModel> { expectedUser };

		_userCursor.Setup(_ => _.Current).Returns(_users);

		var sut = CreateRepository();

		// Act
		await sut.UpVoteAsync(expected.Id, expectedUser.Id);

		// Assert
		expected.UserVotes.Count.Should().Be(0);

	}

}
