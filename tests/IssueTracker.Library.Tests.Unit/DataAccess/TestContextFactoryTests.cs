namespace IssueTracker.Library.DataAccess;

public class TestContextFactoryTests
{

	private readonly IOptions<DatabaseSettings> _options;

	public TestContextFactoryTests()
	{

		_options = TestFixtures.Settings();

	}

	[Fact]
	public void TestContextFactory_With_Valid_Data_Should_Return_A_Context_Test()
	{
		// Arrange

		// Act

		var context = Substitute.For<TestContextFactory>(_options);

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

		var context = Substitute.For<TestContextFactory>(_options);

		// Assert

		Assert.Throws<ArgumentException>(() => context.GetCollection<UserModel>(""));
	}

	[Fact]
	public void GetCollection_With_ValidName_Should_ReturnACollection_Test()
	{
		// Arrange

		// Act

		var context = Substitute.For<TestContextFactory>(_options);
		var myCollection =
			context.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));

		// Assert

		myCollection.Should().NotBeNull();
		myCollection.CollectionNamespace.CollectionName.Should().BeSameAs("users");
	}
}