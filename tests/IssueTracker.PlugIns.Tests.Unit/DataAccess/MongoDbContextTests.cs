﻿
namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
public class MongoDbContextTests
{

	private readonly IMongoDbContextFactory _sut;

	public MongoDbContextTests()
	{

		const string connectionString = "mongodb://test123";
		const string databaseName = "TestDb";

		DatabaseSettings settings = new DatabaseSettings(connectionString, databaseName)
		{
			ConnectionString = connectionString,
			DatabaseName = databaseName
		};


		_sut = Substitute.For<MongoDbContextFactory>(settings);

	}

	[Fact]
	public void MongoDbContext_With_Valid_Data_Should_Return_A_Context_Test()
	{

		// Arrange
		const string expectedConnectionString = "mongodb://test123";
		const string expectedDbName = "TestDb";

		// Act

		// Assert
		_sut.Should().NotBeNull();
		_sut.Client.Should().NotBeNull();
		_sut.DbName.Should().Be(expectedDbName);
		_sut.ConnectionString.Should().Be(expectedConnectionString);

	}

	[Fact]
	public void GetCollection_With_EmptyName_Should_Fail_Test()
	{

		// Arrange

		// Act

		// Assert
		Assert.Throws<ArgumentException>(() => _sut.GetCollection<UserModel>(""));

	}

	[Fact]
	public void GetCollection_With_ValidName_Should_ReturnACollection_Test()
	{

		// Arrange

		// Act
		var myCollection =
			_sut.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));

		// Assert
		myCollection.Should().NotBeNull();
		myCollection.CollectionNamespace.CollectionName.Should().BeSameAs("users");

	}

}
