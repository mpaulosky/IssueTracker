namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterConnections(this IServiceCollection services, ConfigurationManager config)
	{
		string connectionString = config.GetSection("MongoDbSettings:ConnectionStrings").Value ?? string.Empty;
		string databaseName = config.GetSection("MongoDbSettings:DatabaseName").Value ?? string.Empty;

		services.AddSingleton<IDatabaseSettings, DatabaseSettings>();

		return services;

	}

}
