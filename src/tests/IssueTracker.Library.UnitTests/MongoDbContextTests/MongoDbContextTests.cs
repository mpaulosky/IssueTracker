using FluentAssertions;

using IssueTracker.Library.UnitTests.Fixtures;

using IssueTrackerLibrary.DataAccess;
using IssueTrackerLibrary.Models;

using Microsoft.Extensions.Options;

using NSubstitute;

using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

using static IssueTrackerLibrary.Models.CollectionNames;

namespace IssueTracker.Library.UnitTests.MongoDbContextTests;

[ExcludeFromCodeCoverage]
public class MongoDbContextTests
{
	private readonly IOptions<IssueTrackerDatabaseSettings> _options;
	
	public MongoDbContextTests()
	{
		_options = TestFixtures.Settings();
	}

	[Fact()]
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

	[Fact()]
	public void GetCollection_With_EmptyName_Should_Fail_Test()
	{
		// Arrange

		// Act
		
		var context = Substitute.For<MongoDbContext>(_options);

		// Assert
		
		Assert.Throws<ArgumentException>(() => context.GetCollection<User>(""));
	}

	[Fact()]
	public void GetCollection_With_ValidName_Should_ReturnACollection_Test()
	{
		// Arrange

		// Act
		
		var context = Substitute.For<MongoDbContext>(_options);
		var myCollection = context.GetCollection<User>(GetCollectionName(nameof(User)));
		
		// Assert
		
		myCollection.Should().NotBeNull();
		myCollection.CollectionNamespace.CollectionName.Should().BeSameAs("users");
	}
}