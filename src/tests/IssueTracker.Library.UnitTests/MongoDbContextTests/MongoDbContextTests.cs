using FluentAssertions;

using IssueTrackerLibrary.DataAccess;
using IssueTrackerLibrary.Models;

using Microsoft.Extensions.Options;

using MongoDB.Driver;

using NSubstitute;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Xunit;

namespace IssueTracker.Library.UnitTests.MongoDbContextTests;

[ExcludeFromCodeCoverage]
public class MongoDbContextTests
{
	private readonly IOptions<IssueTrackerDatabaseSettings> _options;
	
	public MongoDbContextTests()
	{
		var settings = new IssueTrackerDatabaseSettings()
		{
			DatabaseName = "TestDb", ConnectionString = "mongodb://tes123"
		};

		_options = Options.Create(settings);
	}

	[Fact()]
	public void MongoDbContext_With_Valid_Data_Should_Return_A_Context_Test()
	{
		// Arrange
		var db = Substitute.For<IMongoDatabase>();
		var client = Substitute.For<IMongoClient>();
		client.GetDatabase(_options.Value.DatabaseName, null).Returns(db);

		// Act
		var context = new MongoDbContext(_options);
		
		// Assert
		_options.Should().NotBeNull();
		context.Should().NotBeNull();
	}

	[Fact()]
	public void GetCollection_With_EmptyName_Should_Fail_Test()
	{
		// Arrange
		var db = Substitute.For<IMongoDatabase>();
		var client = Substitute.For<IMongoClient>();
		client.GetDatabase(_options.Value.DatabaseName, null).Returns(db);

		// Act
		var context = new MongoDbContext(_options);

		// Assert
		Assert.Throws<ArgumentException>(() => context.GetCollection<User>(""));
	}

	[Fact()]
	public void GetCollection_With_ValidName_Should_ReturnACollection_Test()
	{
		// Arrange
		var db = Substitute.For<IMongoDatabase>();
		var client = Substitute.For<IMongoClient>();
		client.GetDatabase(_options.Value.DatabaseName, null).Returns(db);


		// Act
		var context = new MongoDbContext(_options);
		var myCollection = context.GetCollection<User>("User");
		
		// Assert
		myCollection.Should().NotBeNull();
	}
}