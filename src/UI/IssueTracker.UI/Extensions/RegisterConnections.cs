namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterConnections(this IServiceCollection services)
	{

		services.AddSingleton<IDatabaseSettings, DatabaseSettings>();

		return services;

	}

}
