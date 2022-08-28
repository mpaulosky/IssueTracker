//-----------------------------------------------------------------------
// <copyright file="MongoDbContext.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
		Guard.Against.Null(configuration, nameof(configuration));
		
		DbName = Guard.Against.NullOrEmpty(configuration.Value.DatabaseName, nameof(configuration));
		
		string connectionString = Guard.Against.NullOrWhiteSpace(configuration.Value.ConnectionString, nameof(connectionString));
		Client = new MongoClient(connectionString);
		
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
		Guard.Against.NullOrWhiteSpace(name, nameof(name));

		IMongoCollection<T> collection = Guard.Against.Null(Database.GetCollection<T>(name));
		
		return collection;
	}
}