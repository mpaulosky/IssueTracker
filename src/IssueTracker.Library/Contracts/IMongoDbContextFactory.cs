//-----------------------------------------------------------------------
// <copyright file="IMongoDbContext.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IMongoDbContextFactory
{
	IMongoDatabase Database { get; }

	IMongoClient Client { get; }

	string DbName { get; }

	IMongoCollection<T> GetCollection<T>(string name);
}