namespace IssueTracker.Library.Contracts;

public interface IMongoDbContext
{
	IMongoDatabase Database { get; }
	IMongoClient Client { get; }
	string DbName { get; }
	IMongoCollection<T> GetCollection<T>(string name);
}