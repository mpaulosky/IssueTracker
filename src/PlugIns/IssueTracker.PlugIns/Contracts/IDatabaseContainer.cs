namespace IssueTracker.PlugIns.Contracts;

public interface IDatabaseContainer
{
	string GetConnectionString();
	string GetDatabaseName();
	Task StartAsync(CancellationToken cancellationToken);
	Task StopAsync(CancellationToken cancellationToken);
}
