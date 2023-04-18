namespace IssueTracker.PlugIns.Mongo.Contracts;

public interface IDatabaseContainer
{
	string GetConnectionString();
	string GetDatabaseName();
	Task StartAsync();
	Task StopAsync();
}
