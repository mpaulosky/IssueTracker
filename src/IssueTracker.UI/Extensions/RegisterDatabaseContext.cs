//-----------------------------------------------------------------------
// <copyright file="RegisterDatabaseContext.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Extensions;

/// <summary>
/// IServiceCollectionExtensions
/// </summary>
public static partial class IServiceCollectionExtensions
{

	/// <summary>
	/// RegisterDatabaseContext
	/// </summary>
	/// <param name="services">IServiceCollection</param>
	/// <param name="config">ConfigurationManager</param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection RegisterDatabaseContext(this IServiceCollection services, ConfigurationManager config)
	{

		var connectionString = Guard.Against.NullOrEmpty(config.GetValue<string>("MongoDbSettings:ConnectionString"));
		var databaseName = Guard.Against.NullOrEmpty(config.GetValue<string>("MongoDbSettings:DatabaseName"));

		services.AddSingleton<IMongoDbContextFactory>(_ =>
				new MongoDbContextFactory(connectionString, databaseName)
			);

		return services;

	}

}
