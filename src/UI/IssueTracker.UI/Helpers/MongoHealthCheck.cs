//-----------------------------------------------------------------------
// <copyright>
//	File:		MongoHealthCheck.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IssueTracker.UI.Helpers;

public class MongoHealthCheck : IHealthCheck
{
	private readonly IMongoDbContextFactory _factory;

	public MongoHealthCheck(IMongoDbContextFactory factory)
	{
		_factory = factory;
	}

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
		CancellationToken cancellationToken = default)
	{
		var healthCheckResult = await CheckMongoDbConnection();

		return healthCheckResult
			? HealthCheckResult.Healthy("MongoDB health check success")
			: HealthCheckResult.Unhealthy("MongoDB health check failure");
	}

	private async Task<bool> CheckMongoDbConnection()
	{
		try
		{
			await _factory.Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	}
}
