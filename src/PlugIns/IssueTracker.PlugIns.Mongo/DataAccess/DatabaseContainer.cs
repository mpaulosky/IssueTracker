namespace IssueTracker.PlugIns.Mongo.DataAccess;

public sealed class DatabaseContainer : IDatabaseContainer
{

	private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();

	public Task StartAsync()
	{
		return _mongoDbContainer.StartAsync();
	}

	public Task StopAsync()
	{
		return _mongoDbContainer.StopAsync();
	}

	public string GetConnectionString() => _mongoDbContainer.GetConnectionString();

	public string GetDatabaseName() => $"test_{Guid.NewGuid():N}";

	public Task StartAsync(CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
