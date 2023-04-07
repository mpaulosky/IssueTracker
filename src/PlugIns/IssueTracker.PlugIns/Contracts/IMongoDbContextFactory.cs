//-----------------------------------------------------------------------
// <copyright>
//	File:		IMongoDbContextFactory.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Contracts;

public interface IMongoDbContextFactory
{

	IMongoDatabase Database { get; }

	IMongoClient Client { get; }

	string ConnectionString { get; }

	string DbName { get; }

	IMongoCollection<T> GetCollection<T>(string name);

}