namespace IssueTrackerLibrary.DataAccess;

public interface IDbConnection
{
	string DbName { get; }
	string StatusCollectionName { get; }
	string UserCollectionName { get; }
	string IssueCollectionName { get; }
	string CommentCollectionName { get; }
	MongoClient Client { get; }
	IMongoCollection<StatusModel> StatusCollection { get; }
	IMongoCollection<UserModel> UserCollection { get; }
	IMongoCollection<IssueModel> IssueCollection { get; }
	IMongoCollection<CommentModel> CommentCollection { get; }
}