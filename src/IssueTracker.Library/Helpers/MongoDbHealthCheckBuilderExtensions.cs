//-----------------------------------------------------------------------
// <copyright file="MongoDbHealthCheckBuilderExtensions.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace IssueTracker.Library.Helpers;

/// <summary>
/// Extension methods to configure <see cref="MongoDbHealthCheck"/>.
/// </summary>
public static class MongoDbHealthCheckBuilderExtensions
{
	private const string _name = "mongodb";


	/// <summary>
	/// Add a health check for MongoDb database that list all collections from specified database on <paramref name="mongoDatabaseName"/>.
	/// </summary>
	/// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
	/// <param name="mongodbConnectionString">The MongoDb connection string to be used.</param>
	/// <param name="mongoDatabaseName">The Database name to check.</param>
	/// <param name="name">The health check name. Optional. If <c>null</c> the type name 'mongodb' will be used for the name.</param>
	/// <param name="failureStatus">
	/// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
	/// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
	/// </param>
	/// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
	/// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
	/// <returns>The specified <paramref name="builder"/>.</returns>
	public static IHealthChecksBuilder AddMongoDb(
			this IHealthChecksBuilder builder,
			string mongodbConnectionString,
			string mongoDatabaseName,
			string name = default,
			HealthStatus failureStatus = default,
			IEnumerable<string> tags = default,
			TimeSpan timeout = default)
	{

		Console.WriteLine($@"IHealthCheckBuilder AddMongoDb mongodbConnectionString: {mongodbConnectionString} mongoDatabaseName: {mongoDatabaseName} name: {name}");
		Guard.Against.NullOrWhiteSpace(mongodbConnectionString, nameof(mongodbConnectionString));
		Guard.Against.NullOrEmpty(mongoDatabaseName, nameof(mongoDatabaseName));

		timeout = new TimeSpan(0, 0, 5);

		return builder.Add(new HealthCheckRegistration(
				name ?? _name,
				sp => new MongoDbHealthCheck(mongodbConnectionString, mongoDatabaseName),
				failureStatus,
				tags,
				timeout));
	}

}