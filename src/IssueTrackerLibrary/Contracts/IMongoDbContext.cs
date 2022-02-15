namespace IssueTrackerLibrary.DataAccess;

public interface IMongoDbContext
{
	IMongoCollection<T> GetCollection<T>(string name);
}