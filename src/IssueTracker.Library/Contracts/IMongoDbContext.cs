namespace IssueTrackerLibrary.Contracts;

public interface IMongoDbContext
{
	string DbName { get; }
	
	MongoClient Client { get; }
	
	IMongoCollection<T> GetCollection<T>(string name);
}