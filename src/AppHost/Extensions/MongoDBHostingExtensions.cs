// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     MongoDBHostingExtensions.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

namespace AppHost.Extensions;

/// <summary>
/// Extension methods for MongoDB hosting with dashboard management commands.
/// </summary>
public static class MongoDBHostingExtensions
{
	/// <summary>
	/// Adds MongoDB to the distributed application with management dashboard commands.
	/// </summary>
	/// <param name="builder">The distributed application builder.</param>
	/// <param name="name">The name of the MongoDB resource.</param>
	/// <param name="databaseName">The default database name. Default is "IssueTrackerDb".</param>
	/// <returns>The MongoDB server resource builder for further configuration.</returns>
	public static IResourceBuilder<MongoDBServerResource> AddMongoDBWithManagement(
		this IDistributedApplicationBuilder builder,
		string name,
		string databaseName = "IssueTrackerDb")
	{
		ArgumentNullException.ThrowIfNull(builder);
		ArgumentException.ThrowIfNullOrWhiteSpace(name);
		ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);

		// Add MongoDB container resource with Aspire API
		var mongodb = builder.AddMongoDB(name)
			.WithDataVolume()
			.WithHealthCheck(name);

		// Add the default database
		var database = mongodb.AddDatabase(databaseName);

		// Add dashboard command to clear all collections in the database
		var clearDataOptions = new CommandOptions
		{
			UpdateState = context => ResourceCommandState.Enabled,
			IconName = "Delete",
			IconVariant = IconVariant.Filled,
			IsHighlighted = false,
			Description = $"Clears all collections in the '{databaseName}' database"
		};

		mongodb.WithCommand(
			name: "clear-data",
			displayName: "Clear All Data",
			executeCommand: async context =>
			{
				var connectionString = await database.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);
				
				if (string.IsNullOrWhiteSpace(connectionString))
				{
					return CommandResults.Failure();
				}

				try
				{
					var client = new MongoClient(connectionString);
					var db = client.GetDatabase(databaseName);
					var collections = await db.ListCollectionNamesAsync(cancellationToken: context.CancellationToken);

					var clearedCount = 0;
					while (await collections.MoveNextAsync(context.CancellationToken))
					{
						foreach (var collectionName in collections.Current)
						{
							await db.DropCollectionAsync(collectionName, context.CancellationToken);
							clearedCount++;
						}
					}

					return CommandResults.Success();
				}
				catch (MongoException)
				{
					// MongoDB-specific errors (connection, authentication, etc.)
					return CommandResults.Failure();
				}
				catch (OperationCanceledException)
				{
					// Operation was cancelled by user or timeout
					return CommandResults.Failure();
				}
			},
			commandOptions: clearDataOptions);

		// Add dashboard command to drop the entire database
		var dropDatabaseOptions = new CommandOptions
		{
			UpdateState = context => ResourceCommandState.Enabled,
			IconName = "DeleteForever",
			IconVariant = IconVariant.Filled,
			IsHighlighted = true,
			Description = $"Drops the entire '{databaseName}' database",
			ConfirmationMessage = $"Are you sure you want to drop the database '{databaseName}'? This action cannot be undone."
		};

		mongodb.WithCommand(
			name: "drop-database",
			displayName: "Drop Database",
			executeCommand: async context =>
			{
				var connectionString = await database.Resource.ConnectionStringExpression.GetValueAsync(context.CancellationToken);
				
				if (string.IsNullOrWhiteSpace(connectionString))
				{
					return CommandResults.Failure();
				}

				try
				{
					var client = new MongoClient(connectionString);
					await client.DropDatabaseAsync(databaseName, context.CancellationToken);
					return CommandResults.Success();
				}
				catch (MongoException)
				{
					// MongoDB-specific errors (connection, authentication, etc.)
					return CommandResults.Failure();
				}
				catch (OperationCanceledException)
				{
					// Operation was cancelled by user or timeout
					return CommandResults.Failure();
				}
			},
			commandOptions: dropDatabaseOptions);

		return mongodb;
	}
}
