namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests
{
	private readonly Mock<IAsyncCursor<UserModel>> _cursor;
	private readonly Mock<IMongoCollection<UserModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<UserModel> _list = new();

	public UserRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();
	}

	private UserRepository CreateRepository()
	{
		return new UserRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "CreateUser with a valid user")]
	public async Task CreateUser_With_Valid_User_Should_Insert_A_New_User_TestAsync()
	{
		// Arrange
		var newUser = FakeUser.GetNewUser(true);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.CreateAsync(newUser);

		// Assert
		//Verify if InsertOneAsync is called once 
		_mockCollection.Verify(c => c
			.InsertOneAsync(
				newUser,
				null,
				default), Times.Once);
	}

	[Fact(DisplayName = "GetUser With a Valid Id")]
	public async Task GetUser_With_Valid_Id_Should_Returns_One_User_Test()
	{
		// Arrange
		var expected = FakeUser.GetNewUser(true);

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetAsync(expected.Id);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);

		//Verify if FindAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<UserModel>>(),
				It.IsAny<FindOptions<UserModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "GetUser From Authentication")]
	public async Task GetUserFromAuthentication_With_Valid_ObjectIdentifier_Should_Returns_One_User_Test()
	{
		// Arrange
		var expected = FakeUser.GetNewUser(true);

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		//Act
		var result = await sut.GetFromAuthenticationAsync(expected.ObjectIdentifier).ConfigureAwait(false);

		//Assert 
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(expected);


		//Verify if FindAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(
				It.IsAny<FilterDefinition<UserModel>>(),
				It.IsAny<FindOptions<UserModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Get Users")]
	public async Task GetUsers_With_Valid_Context_Should_Return_A_List_Of_Users_Test()
	{
		// Arrange
		const int expectedCount = 3;
		var expected = FakeUser.GetUsers(expectedCount).ToList();

		_list = new List<UserModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var results = (await sut.GetAllAsync().ConfigureAwait(false)).ToList();

		// Assert
		results.Should().NotBeNull();
		results.Should().HaveCount(expectedCount);

		//Verify if FindAsync is called once
		_mockCollection.Verify(c => c
			.FindAsync(
				FilterDefinition<UserModel>.Empty,
				It.IsAny<FindOptions<UserModel>>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Update User with a valid Id and User")]
	public async Task UpdateUser_With_A_Valid_Id_And_User_Should_UpdateUser_Test()
	{
		// Arrange
		var expected = FakeUser.GetNewUser(true);

		var updatedUser = FakeUser.GetNewUser(true);
		updatedUser.Id = expected.Id;
		updatedUser.Archived = true;

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.UpdateAsync(updatedUser.Id, updatedUser);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(
					It.IsAny<FilterDefinition<UserModel>>(),
					updatedUser,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact(DisplayName = "Archive User")]
	public async Task ArchiveUser_With_A_Valid_Id_And_User_Should_ArchiveUser_Test()
	{
		// Arrange
		var expected = FakeUser.GetNewUser(true);

		await _mockCollection.Object.InsertOneAsync(expected);

		var updatedUser = FakeUser.GetNewUser(true);
		updatedUser.Archived = true;

		_list = new List<UserModel> { updatedUser };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c
			.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(updatedUser);

		// Assert
		_mockCollection.Verify(
			c => c
				.ReplaceOneAsync(
					It.IsAny<FilterDefinition<UserModel>>(),
					updatedUser,
					It.IsAny<ReplaceOptions>(),
					It.IsAny<CancellationToken>()), Times.Once);
	}
}
