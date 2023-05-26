namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests
{

	private readonly Mock<IAsyncCursor<UserModel>> _cursor;
	private readonly Mock<IMongoCollection<UserModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<UserModel> _list = new();

	public UserRepositoryTests()
	{

		_cursor = TestFixturesMongo.GetMockCursor(_list);

		_mockCollection = TestFixturesMongo.GetMockCollection(_cursor);

		_mockContext = TestFixturesMongo.GetMockContext();

	}

	private UserRepository CreateRepository()
	{

		return new UserRepository(_mockContext.Object);

	}

	[Fact(DisplayName = "Archive User Test")]
	public async Task ArchiveAsync_With_Valid_User_Should_Archive_the_User_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetNewUser(true);

		var updatedUser = FakeUser.GetNewUser(true);
		updatedUser.Archived = true;

		await _mockCollection.Object.InsertOneAsync(expected);

		_mockContext.Setup(c => c
				.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedUser);

		// Assert

		_mockCollection.Verify(
			c => c.
				ReplaceOneAsync(It.IsAny<FilterDefinition<UserModel>>(),
					updatedUser,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);

	}
	[Fact(DisplayName = "Create User Test")]
	public async Task CreateAsync_With_Valid_User_Should_Insert_A_New_User_TestAsync()
	{

		// Arrange
		var newUser = FakeUser.GetNewUser();

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newUser);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(newUser, null, default), Times.Once);

	}

	[Fact(DisplayName = "Get User By Id")]
	public async Task GetAsync_With_Valid_Id_Should_Returns_One_User_TestAsync()
	{

		// Arrange
		var expected = FakeUser.GetUsers(1).First();

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);


		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<UserModel>>(),
			It.IsAny<FindOptions<UserModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users By Authentication Id")]
	public async Task GetByAuthenticationIdAsync_With_Valid_User_Id_Should_Return_A_List_Of_Users_TestAsync()
	{

		// Arrange
		const int expectedCount = 1;
		var expected = FakeUser.GetUsers(expectedCount).First();
		expected.Archived = false;

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
				.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var results = await sut.GetByAuthenticationIdAsync(expected.ObjectIdentifier);

		//Assert 
		results.Should().NotBeNull();
		results.Should().BeEquivalentTo(expected);

		//Verify if InsertOneAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<UserModel>>(),
				It.IsAny<FindOptions<UserModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users Without Archived")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Users_Without_Archived_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeUser.GetUsers(expectedCount).ToList();
		foreach (var User in expected)
		{
			User.Archived = false;
		}

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<UserModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync().ConfigureAwait(false))!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);
		results.Any(x => x.Archived).Should().BeFalse();

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<UserModel>>(),
			It.IsAny<FindOptions<UserModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Users With Archived")]
	public async Task GetAllAsync_With_Valid_Context_Should_Return_A_List_Of_Users_With_Archived_TestAsync()
	{

		// Arrange
		const int expectedCount = 5;
		var expected = FakeUser.GetUsers(expectedCount).ToList();

		await _mockCollection.Object.InsertManyAsync(expected);

		_mockContext.Setup(c => c
				.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		_list = new List<UserModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync(true).ConfigureAwait(false))!.ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);
		results.Should().BeEquivalentTo(expected);
		results.Any(x => x.Archived).Should().BeTrue();

		_mockCollection.Verify(c => c
			.FindAsync(It.IsAny<FilterDefinition<UserModel>>(),
			It.IsAny<FindOptions<UserModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update User Test")]
	public async Task UpdateAsync_With_A_Valid_Id_And_User_Should_UpdateUser_Test()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		expected.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
		expected.Archived = false;

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedUser = FakeUser.GetNewUser();

		updatedUser.Id = expected.Id;
		updatedUser.FirstName = expected.FirstName;
		updatedUser.LastName = expected.LastName;
		updatedUser.DisplayName = expected.DisplayName;
		updatedUser.Archived = true;

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>()))
			.Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedUser).ConfigureAwait(false);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(It.IsAny<FilterDefinition<UserModel>>(), updatedUser,
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

}
