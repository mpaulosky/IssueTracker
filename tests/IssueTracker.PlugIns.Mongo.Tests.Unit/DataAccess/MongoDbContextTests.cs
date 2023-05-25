
namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
public class MongoDbContextTests
{

	const string ConnectionString = "mongodb://test123";
	const string DatabaseName = "TestDb";

	private static MongoDbContextFactory UnitUnderTest()
	{

		DatabaseSettings settings = new DatabaseSettings(ConnectionString, DatabaseName)
		{
			ConnectionString = ConnectionString,
			DatabaseName = DatabaseName
		};

		return Substitute.For<MongoDbContextFactory>(settings);

	}

	[Fact]
	public void MongoDbContext_With_Valid_Data_Should_Return_A_Context_Test()
	{

		// Arrange
		var sut = UnitUnderTest();

		// Act

		// Assert
		sut.Should().NotBeNull();
		sut.Client.Should().NotBeNull();
		sut.ConnectionString.Should().Be(ConnectionString);
		sut.DbName.Should().Be(DatabaseName);

	}

	[Theory]
	[InlineData(null, "Value cannot be null. (Parameter 'name')")]
	[InlineData("", "The value cannot be an empty string. (Parameter 'name')")]
	public void GetCollection_With_Invalid_Name_Should_Fail_Test(string value, string expectedMessage)
	{

		// Arrange
		var sut = UnitUnderTest();

		// Act
		Action act = () => sut.GetCollection<UserModel>(value);

		// Assert
		act.Should()
			.Throw<ArgumentException>()
			.WithParameterName("name")
			.WithMessage(expectedMessage);

	}

	[Fact]
	public void GetCollection_With_ValidName_Should_ReturnACollection_Test()
	{

		// Arrange
		var sut = UnitUnderTest();

		// Act
		var myCollection =
			sut.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));

		// Assert
		myCollection.Should().NotBeNull();
		myCollection.CollectionNamespace.CollectionName.Should().BeSameAs("users");

	}

}
