namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterConnections(this IServiceCollection services, ConfigurationManager config)
	{

		services.AddSingleton<IDatabaseSettings, DatabaseSettings>();

		return services;

	}

}
