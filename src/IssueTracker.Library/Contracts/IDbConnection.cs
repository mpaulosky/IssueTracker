namespace IssueTrackerLibrary.Contracts;

public interface IDbConnection
{
	string DbName { get; }
	MongoClient Client { get; }

	string StatusCollectionName { get; }
	string UserCollectionName { get; }
	string IssueCollectionName { get; }
	string CommentCollectionName { get; }
	IMongoCollection<Status> StatusCollection { get; }
	IMongoCollection<User> UserCollection { get; }
	IMongoCollection<Issue> IssueCollection { get; }
	IMongoCollection<Comment> CommentCollection { get; }
}