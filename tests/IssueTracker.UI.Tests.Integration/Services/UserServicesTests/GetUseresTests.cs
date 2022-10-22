namespace IssueTracker.Library.Services.UserServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetUseresTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserService _sut;

	public GetUseresTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(UserModel)));

		var repo = (IUserRepository)_factory.Services.GetRequiredService(typeof(IUserRepository));
		
		_sut = new UserService(repo);

	}

	[Fact]
	public async Task GetUseres_With_ValidData_Should_ReturnUseres_Test()
	{

		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateUser(expected);

		// Act
		var results = await _sut.GetUsers();

		// Assert
		results.Count.Should().Be(1);
		results.First().DisplayName.Should().Be(expected.DisplayName);
		results.First().FirstName.Should().Be(expected.FirstName);
		results.First().LastName.Should().Be(expected.LastName);

	}

}
