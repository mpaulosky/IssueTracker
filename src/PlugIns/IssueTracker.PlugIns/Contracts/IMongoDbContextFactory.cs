// Copyright (c) 2023. All rights reserved.
// File Name :     IMongoDbContextFactory.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns

namespace IssueTracker.PlugIns.Contracts;

public interface IMongoDbContextFactory
{
	IMongoDatabase Database { get; }

	IMongoClient Client { get; }

	string ConnectionString { get; }

	string DbName { get; }

	IMongoCollection<T> GetCollection<T>(string name);
}