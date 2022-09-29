//-----------------------------------------------------------------------
// <copyright file="TestConnectionFactory.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

/// <summary>
///		TestConnectionFactory class
/// </summary>
/// <seealso cref="IssueTracker.Library.Contracts.IMongoDbContextFactory" />
public class TestContextFactory : IMongoDbContextFactory
{

	/// <summary>
	///		Initializes a new instance of the <see cref="TestContextFactory" /> class.
	/// </summary>
	/// <param name="configuration">The configuration.</param>
	public TestContextFactory(IOptions<DatabaseSettings> configuration)
	{

		DbName = configuration.Value.DatabaseName;

		var connectionString = configuration.Value.ConnectionString;

		Client = new MongoClient(connectionString);

		Database = Client.GetDatabase(DbName);

	}

	/// <summary>
	///		Gets the database.
	/// </summary>
	/// <value>
	///		The database.
	/// </value>
	public IMongoDatabase Database { get; }

	/// <summary>
	///		Gets the client.
	/// </summary>
	/// <value>
	///		The client.
	/// </value>
	public IMongoClient Client { get; }

	/// <summary>
	///		Gets the name of the database.
	/// </summary>
	/// <value>
	///		The name of the database.
	/// </value>
	public string DbName { get; }

	/// <summary>
	///		Gets the collection.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name">The name of the collection.</param>
	/// <returns>A IMongoCollection of type T</returns>
	public IMongoCollection<T> GetCollection<T>(string name)
	{

		var collection = Database.GetCollection<T>(name);

		return collection;

	}

}