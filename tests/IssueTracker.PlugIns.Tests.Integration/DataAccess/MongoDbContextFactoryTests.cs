// Copyright (c) 2023. All rights reserved.
// File Name :     MongoDbContextFactoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

using MongoDB.Bson;

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class MongoDbContextFactoryTests : IAsyncLifetime
{
	private const string CleanupValue = "";

	private readonly IssueTrackerTestFactory _factory;

	public MongoDbContextFactoryTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		_factory.DbContext = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact]
	public void GetCollection_With_Valid_DbContext_Should_Return_Value_Test()
	{
		// Arrange
		const string name = "users";

		// Act
		IMongoCollection<UserModel> result =
			_factory.DbContext!.GetCollection<UserModel>(name);

		// Assert
		result.Should().NotBeNull();
	}

	[Fact]
	public void ConnectionStateReturnsOpen()
	{
		// Given
		IMongoClient client = _factory.DbContext!.Client;

		// When
		using IAsyncCursor<BsonDocument>? databases = client.ListDatabases();

		// Then
		Assert.Contains(databases.ToEnumerable(),
			database => database.TryGetValue("name", out BsonValue? name) && "admin".Equals(name.AsString));
	}

	[Fact]
	public async Task Be_healthy_if_mongodb_is_available()
	{
		// Arrange
		TestServer sut = _factory.Server;

		// Act
		HttpResponseMessage response = await sut.CreateRequest("/health").GetAsync();

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}
}