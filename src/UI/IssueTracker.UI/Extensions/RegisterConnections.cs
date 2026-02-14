// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RegisterConnections.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterConnections(this IServiceCollection services, ConfigurationManager config)
	{
		IConfigurationSection section = config.GetSection("MongoDbSettings");
		DatabaseSettings? mongoSettings = section.Get<DatabaseSettings>();
		
		if (mongoSettings is null)
		{
			throw new InvalidOperationException("MongoDbSettings configuration section is missing or invalid.");
		}
		
		services.AddSingleton<IDatabaseSettings>(mongoSettings);

		return services;
	}
}