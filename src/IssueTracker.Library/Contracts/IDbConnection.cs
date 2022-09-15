//-----------------------------------------------------------------------
// <copyright file="IDbConnection.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IDbConnectionFactory
{
	string DbName { get; }

	MongoClient Client { get; }

	string StatusCollectionName { get; }

	string UserCollectionName { get; }

	string IssueCollectionName { get; }

	string CommentCollectionName { get; }

	IMongoCollection<StatusModel> StatusCollection { get; }

	IMongoCollection<UserModel> UserCollection { get; }

	IMongoCollection<IssueModel> IssueCollection { get; }

	IMongoCollection<CommentModel> CommentCollection { get; }
}