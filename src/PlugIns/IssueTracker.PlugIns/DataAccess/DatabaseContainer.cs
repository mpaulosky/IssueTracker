namespace IssueTracker.PlugIns.DataAccess;

public sealed class DatabaseContainer : IDatabaseContainer
{

	private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();

	public Task StartAsync(CancellationToken cancellationToken)
	{
		return _mongoDbContainer.StartAsync(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return _mongoDbContainer.StopAsync(cancellationToken);
	}

	public string GetConnectionString() => _mongoDbContainer.GetConnectionString();

	public string GetDatabaseName() => $"test_{Guid.NewGuid():N}";

}
