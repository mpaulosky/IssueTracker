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
	///		MongoDbContextFactpru constructor
	/// </summary>
	/// <param name="connectionString">Connection String</param>
	/// <param name="databaseName">Database Name</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentException"></exception>"
	public TestContextFactory(string connectionString, string databaseName)
	{

		ConnectionString = Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));

		DbName = Guard.Against.NullOrWhiteSpace(databaseName, nameof(databaseName));

		Client = new MongoClient(ConnectionString);

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
	///		Gets the Connection String to the Database
	/// </summary>
	public string ConnectionString { get; }

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