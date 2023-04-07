//-----------------------------------------------------------------------
// <copyright>
//	File:		MongoDbContextFactory.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
///		MongoDbContext class
/// </summary>
public class MongoDbContextFactory : IMongoDbContextFactory
{

	/// <summary>
	///		MongoDbContextFactory constructor
	/// </summary>
	/// <param name="connectionString">Connection String</param>
	/// <param name="databaseName">Database Name</param>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="ArgumentException"></exception>"
	public MongoDbContextFactory(string connectionString, string databaseName)
	{

		ConnectionString = Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));

		DbName = Guard.Against.NullOrWhiteSpace(databaseName, nameof(databaseName));

		Client = new MongoClient(ConnectionString);

		Database = Client.GetDatabase(DbName);

	}

	/// <summary>
	/// Gets the database.
	/// </summary>
	/// <value>
	/// The database.
	/// </value>
	public IMongoDatabase Database { get; }

	/// <summary>
	/// Gets the client.
	/// </summary>
	/// <value>
	/// The client.
	/// </value>
	public IMongoClient Client { get; }

	/// <summary>
	/// Gets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	public string ConnectionString { get; }

	/// <summary>
	/// Gets the name of the database.
	/// </summary>
	/// <value>
	/// The name of the database.
	/// </value>
	public string DbName { get; }

	/// <summary>
	///		GetCollection method
	/// </summary>
	/// <param name="name">string</param>
	/// <typeparam name="T"></typeparam>
	/// <returns>IMongoCollection</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public IMongoCollection<T> GetCollection<T>(string name)
	{

		Guard.Against.NullOrWhiteSpace(name, nameof(name));

		IMongoCollection<T> collection = Guard.Against.Null(Database.GetCollection<T>(name));

		return collection;

	}

}
