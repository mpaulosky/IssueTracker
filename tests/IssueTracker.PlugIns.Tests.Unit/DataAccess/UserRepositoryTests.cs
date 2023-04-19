namespace IssueTracker.CoreBusiness.DataAccess;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests
{
	private readonly Mock<IAsyncCursor<UserModel>> _cursor;
	private readonly Mock<IMongoCollection<UserModel>> _mockCollection;
	private readonly Mock<IMongoDbContextFactory> _mockContext;
	private List<UserModel> _list = new();
	private UserMongoRepository _sut;

	public UserRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new UserMongoRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "CreateUser with a valid user")]
	public async Task CreateUser_With_Valid_User_Should_Insert_A_New_User_TestAsync()
	{
		// Arrange

		UserModel newUser = TestUsers.GetKnownUser();

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserMongoRepository(_mockContext.Object);

		// Act

		await _sut.CreateUserAsync(newUser);

		// Assert

		//Verify if InsertOneAsync is called once 

		_mockCollection.Verify(c => c.InsertOneAsync(newUser, null, default), Times.Once);
	}

	[Fact(DisplayName = "GetUser With a Valid Id")]
	public async Task GetUser_With_Valid_Id_Should_Returns_One_User_Test()
	{
		// Arrange

		UserModel expected = TestUsers.GetKnownUser();

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserMongoRepository(_mockContext.Object);

		//Act

		UserModel result = await _sut.GetUserByIdAsync(expected!.Id!);

		//Assert 

		result.Should().NotBeNull();

		//Verify if FindAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<UserModel>>(),
			It.IsAny<FindOptions<UserModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result!.FirstName!.Length.Should().BeGreaterThan(1);
		result!.EmailAddress!.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "GetUser From Authentication")]
	public async Task GetUserFromAuthentication_With_Valid_ObjectIdentifier_Should_Returns_One_User_Test()
	{
		// Arrange
		UserModel expected = TestUsers.GetKnownUser();

		_list = new List<UserModel> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserMongoRepository(_mockContext.Object);

		//Act

		UserModel result = await _sut.GetUserByAuthenticationIdAsync(expected!.ObjectIdentifier!).ConfigureAwait(false);

		//Assert 

		result.Should().NotBeNull();

		//Verify if FindAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<UserModel>>(),
			It.IsAny<FindOptions<UserModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact(DisplayName = "Get Users")]
	public async Task GetUsers_With_Valid_Context_Should_Return_A_List_Of_Users_Test()
	{
		// Arrange

		var expected = TestUsers.GetUsers().ToList();

		_list = new List<UserModel>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserMongoRepository(_mockContext.Object);

		// Act

		IEnumerable<UserModel> result = await _sut.GetUsersAsync().ConfigureAwait(false);

		// Assert

		//Verify if FindAsync is called once

		_mockCollection.Verify(c => c.FindAsync(FilterDefinition<UserModel>.Empty,
			It.IsAny<FindOptions<UserModel>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
	}

	[Fact(DisplayName = "Update User with a valid Id and User")]
	public async Task UpdateUser_With_A_Valid_Id_And_User_Should_UpdateUser_Test()
	{
		// Arrange

		UserModel expected = TestUsers.GetKnownUser();

		UserModel updatedUser = TestUsers.GetUser(userId: expected!.Id!, expected!.ObjectIdentifier!, "James",
			expected!.LastName!,
			expected!.DisplayName!, "james.test@test.com");

		_list = new List<UserModel> { updatedUser };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserMongoRepository(_mockContext.Object);

		// Act

		await _sut.UpdateUserAsync(updatedUser);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<UserModel>>(), updatedUser, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}


	[Fact(DisplayName = "Archive User")]
	public async Task ArchiveUser_With_A_Valid_Id_And_User_Should_ArchiveUser_Test()
	{
		// Arrange

		UserModel expected = TestUsers.GetKnownUser();

		await _mockCollection.Object.InsertOneAsync(expected);

		UserModel updatedUser = TestUsers.GetKnownUser();
		updatedUser.Archived = true;

		_list = new List<UserModel> { updatedUser };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<UserModel>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserMongoRepository(_mockContext.Object);

		// Act

		await _sut.UpdateUserAsync(updatedUser);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<UserModel>>(), updatedUser, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}

}
