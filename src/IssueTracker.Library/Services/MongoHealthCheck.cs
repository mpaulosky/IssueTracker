using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IssueTracker.Library.Services;
public class MongoHealthCheck : IHealthCheck
{
	private IMongoDatabase Db { get; set; }
	public MongoClient MongoClient { get; set; }

	public MongoHealthCheck(IOptions<DatabaseSettings> configuration)
	{
		MongoClient = new MongoClient(configuration.Value.ConnectionString);
		Db = MongoClient.GetDatabase(configuration.Value.DatabaseName);
	}

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
	{
		var healthCheckResultHealthy = await CheckMongoDBConnectionAsync();


		if (healthCheckResultHealthy)
		{
			return HealthCheckResult.Healthy("MongoDB health check success");
		}

		return HealthCheckResult.Unhealthy("MongoDB health check failure"); ;
	}

	private async Task<bool> CheckMongoDBConnectionAsync()
	{
		try
		{
			await Db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
		}

		catch (Exception)
		{
			return false;
		}

		return true;
	}
}