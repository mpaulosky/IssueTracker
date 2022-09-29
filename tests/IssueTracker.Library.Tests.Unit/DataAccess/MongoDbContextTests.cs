namespace IssueTracker.Library.DataAccess;

[ExcludeFromCodeCoverage]
public class MongoDbContextTests
{
	private readonly IOptions<DatabaseSettings> _options;

	public MongoDbContextTests()
	{
		_options = TestFixtures.Settings();
	}

	[Fact]
	public void MongoDbContext_With_Valid_Data_Should_Return_A_Context_Test()
	{
		// Arrange

		// Act

		var context = Substitute.For<MongoDbContext>(_options);

		// Assert

		context.Should().NotBeNull();
		context.Client.Should().NotBeNull();
		context.DbName.Should().Be("TestDb");
	}

	[Fact]
	public void GetCollection_With_EmptyName_Should_Fail_Test()
	{
		// Arrange

		// Act

		var context = Substitute.For<MongoDbContext>(_options);

		// Assert

		Assert.Throws<ArgumentException>(() => context.GetCollection<UserModel>(""));
	}

	[Fact]
	public void GetCollection_With_ValidName_Should_ReturnACollection_Test()
	{
		// Arrange

		// Act

		var context = Substitute.For<MongoDbContext>(_options);
		var myCollection =
			context.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));

		// Assert

		myCollection.Should().NotBeNull();
		myCollection.CollectionNamespace.CollectionName.Should().BeSameAs("users");
	}
}