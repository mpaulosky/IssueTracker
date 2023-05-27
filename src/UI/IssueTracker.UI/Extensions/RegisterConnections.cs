namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterConnections(this IServiceCollection services, ConfigurationManager config)
	{

		var section = config.GetSection("MongoDbSettings");
		var mongoClientConfig = section.Get<MongoDbSettings>();
		services.AddSingleton<IDatabaseSettings>(_ => new DatabaseSettings(mongoClientConfig!.ConnectionStrings!, mongoClientConfig!.DatabaseName!));

		return services;

	}

}
