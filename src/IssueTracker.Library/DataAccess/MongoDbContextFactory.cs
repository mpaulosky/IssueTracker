//-----------------------------------------------------------------------
// <copyright file="MongoDbContext.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

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

	public IMongoDatabase Database { get; }
	public IMongoClient Client { get; }
	public string ConnectionString { get; }
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

		var collection = Guard.Against.Null(Database.GetCollection<T>(name));

		return collection;

	}

}