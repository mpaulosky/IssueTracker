namespace IssueTracker.Library.UnitTests.MongoUsersDataTests;

[ExcludeFromCodeCoverage]
public class MongoUsersServiceTests
{
	private IOptions<DatabaseSettings> _options;

	public MongoUsersServiceTests()
	{
		var settings = new DatabaseSettings()
		{
			DatabaseName = "TestDb", ConnectionString = "mongodb://tes123"
		};

		_options = Options.Create(settings);

	}
	
	/*
	[Fact()]
	public async Task GetUsers_With_StateUnderTest_Should_Return_A_List_of_Users_Test()
	{
		// Arrange
		var _context = Substitute.For<MongoDbContext>(_options);
		
		var expected = new List<UserModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Jim",
				LastName = "Test",
				DisplayName = "jimtest",
				EmailAddress = "jim.test@test.com",
				AuthoredIssues = new List<BasicIssueModel>(),
				VotedOnComments = new List<BasicCommentModel>(),
				AuthoredComments = new List<BasicCommentModel>()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Sam",
				LastName = "Test",
				DisplayName = "samtest",
				EmailAddress = "sam.test@test.com",
				AuthoredIssues = new List<BasicIssueModel>(),
				VotedOnComments = new List<BasicCommentModel>(),
				AuthoredComments = new List<BasicCommentModel>()
			},
			new ()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Tim",
				LastName = "Test",
				DisplayName = "timtest",
				EmailAddress = "tim.test@test.com",
				AuthoredIssues = new List<BasicIssueModel>(),
				VotedOnComments = new List<BasicCommentModel>(),
				AuthoredComments = new List<BasicCommentModel>()
			},
		};
		await _context.UserCollection.InsertManyAsync(expected).ConfigureAwait(false);
		
		var _sut = new MongoUserService(_context);

		// Act
		var users = await _sut.GetUsers().ConfigureAwait(false);
		
		// Assert
		users.Should().NotBeNull();
	}
*/
}