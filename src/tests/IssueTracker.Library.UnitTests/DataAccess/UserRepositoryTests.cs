using MongoDB.Driver;

namespace IssueTracker.Library.UnitTests.DataAccess;

[ExcludeFromCodeCoverage]
public class UserRepositoryTests
{
	private UserRepository _sut;
	private readonly Mock<IMongoCollection<User>> _mockCollection;
	private readonly Mock<IMongoDbContext> _mockContext;
	private readonly Mock<IAsyncCursor<User>> _cursor;
	private List<User> _list = new();

	public UserRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();

		_sut = new UserRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "CreateUser with a valid user")]
	public async Task CreateUser_With_Valid_User_Should_Insert_A_New_User_TestAsync()
	{
		// Arrange

		var newUser = TestUsers.GetKnownUser();

		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserRepository(_mockContext.Object);

		// Act

		await _sut.CreateUser(newUser);

		// Assert

		//Verify if InsertOneAsync is called once 

		_mockCollection.Verify(c => c.InsertOneAsync(newUser, null, default), Times.Once);
	}

	[Fact(DisplayName = "GetUser With a Valid Id")]
	public async Task GetUser_With_Valid_Id_Should_Returns_One_User_Test()
	{
		// Arrange

		var expected = TestUsers.GetKnownUser();

		_list = new List<User> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetUser(expected.Id);

		//Assert 

		result.Should().NotBeNull();

		//Verify if FindAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
		result.AuthoredComments.Should().NotBeNull();
		result.AuthoredIssues.Should().NotBeNull();
		result.VotedOnComments.Should().NotBeNull();
		result.FirstName.Length.Should().BeGreaterThan(1);
		result.EmailAddress.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "GetUser From Authentication")]
	public async Task GetUserFromAuthentication_With_Valid_ObjectIdentifier_Should_Returns_One_User_Test()
	{
		// Arrange
		var expected = TestUsers.GetKnownUser();

		_list = new List<User> { expected };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserRepository(_mockContext.Object);

		//Act

		var result = await _sut.GetUserFromAuthentication(expected.ObjectIdentifier).ConfigureAwait(false);

		//Assert 

		result.Should().NotBeNull();

		//Verify if FindAsync is called once

		_mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact(DisplayName = "Get Users")]
	public async Task GetUsers_With_Valid_Context_Should_Return_A_List_Of_Users_Test()
	{
		// Arrange

		var expected = TestUsers.GetUsers().ToList();

		_list = new List<User>(expected);

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserRepository(_mockContext.Object);

		// Act

		var result = await _sut.GetUsers().ConfigureAwait(false);

		// Assert

		//Verify if FindAsync is called once

		_mockCollection.Verify(c => c.FindAsync(FilterDefinition<User>.Empty,
			It.IsAny<FindOptions<User>>(),
			It.IsAny<CancellationToken>()), Times.Once);

		var items = result.ToList();
		items.ToList().Should().NotBeNull();
		items.ToList().Should().HaveCount(3);
		items[0].VotedOnComments.Should().NotBeNull();
		items[0].AuthoredIssues[0].Id.Should().NotBeNull();
		items[0].AuthoredIssues[0].Issue.Should().NotBeNull();
		items[0].AuthoredComments[0].Id.Should().NotBeNull();
		items[0].AuthoredComments[0].Comment.Should().NotBeNull();
		items[0].AuthoredComments[0].Id.Length.Should().BeGreaterThan(1);
		items[0].AuthoredComments[0].Comment.Length.Should().BeGreaterThan(1);
	}

	[Fact(DisplayName = "Update User with a valid Id and User")]
	public async Task UpdateUser_With_A_Valid_Id_And_User_Should_UpdateUser_Test()
	{
		// Arrange

		var expected = TestUsers.GetKnownUser();

		var updatedUser = TestUsers.GetUser(expected.Id, expected.ObjectIdentifier, "James", expected.LastName,
			expected.DisplayName, "james.test@test.com");

		_list = new List<User> { updatedUser };

		_cursor.Setup(_ => _.Current).Returns(_list);

		_mockContext.Setup(c => c.GetCollection<User>(It.IsAny<string>())).Returns(_mockCollection.Object);

		_sut = new UserRepository(_mockContext.Object);

		// Act

		await _sut.UpdateUser(updatedUser.Id, updatedUser);

		// Assert

		_mockCollection.Verify(
			c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<User>>(), updatedUser, It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
	}
}