//-----------------------------------------------------------------------
// <copyright>
//	File:		MongoDbHealthCheck.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IssueTracker.PlugIns.Helpers;

/// <summary>
/// MongoDbHealthCheck class.
/// </summary>
public class MongoDbHealthCheck : IHealthCheck
{

	private readonly IMongoDbContextFactory _factory;

	public MongoDbHealthCheck(IMongoDbContextFactory factory)
	{
		_factory = factory;
	}

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
	{

		try
		{

			var mongoClient = _factory.Client;

			if (!string.IsNullOrEmpty(_factory.DbName))
			{

				// some users can't list all databases depending on database privileges, with
				// this you can list only collections on specified database.
				// Related with issue #43

				using var cursor = await mongoClient
						.GetDatabase(_factory.DbName)
						.ListCollectionNamesAsync(cancellationToken: cancellationToken);

				await cursor.FirstAsync(cancellationToken);

			}
			else
			{

				using var cursor = await mongoClient.ListDatabaseNamesAsync(cancellationToken);
				await cursor.FirstOrDefaultAsync(cancellationToken);

			}

			return HealthCheckResult.Healthy();

		}
		catch (Exception ex)
		{
			return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
		}

	}

}
