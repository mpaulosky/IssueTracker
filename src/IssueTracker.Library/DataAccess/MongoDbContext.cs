//-----------------------------------------------------------------------
// <copyright file="MongoDbContext.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Options;

namespace IssueTracker.Library.DataAccess;

/// <summary>
///   MongoDbContext class
/// </summary>
public class MongoDbContext : IMongoDbContext
{
	/// <summary>
	///   MongoDbContext constructor
	/// </summary>
	/// <param name="configuration">IOptions of DatabaseSettings</param>
	public MongoDbContext(IOptions<DatabaseSettings> configuration)
	{
		DbName = configuration.Value.DatabaseName;
		Client = new MongoClient(configuration.Value.ConnectionString);
		Database = Client.GetDatabase(DbName);
	}

	public IMongoDatabase Database { get; }
	public IMongoClient Client { get; }
	public string DbName { get; }

	/// <summary>
	///   GetCollection method
	/// </summary>
	/// <param name="name">string</param>
	/// <typeparam name="T"></typeparam>
	/// <returns>IMongoCollection</returns>
	public IMongoCollection<T> GetCollection<T>(string name)
	{
		return Database.GetCollection<T>(name);
	}
}