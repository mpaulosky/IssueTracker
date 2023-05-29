namespace IssueTracker.UI.Extensions;

public static partial class ServiceCollectionExtensions
{

	public static IServiceCollection RegisterConnections(this IServiceCollection services, ConfigurationManager config)
	{

		var section = config.GetSection("MongoDbSettings");
		ArgumentNullException.ThrowIfNull(section);
		DatabaseSettings mongoSettings = section.Get<DatabaseSettings>()!;
		services.AddSingleton<IDatabaseSettings>(mongoSettings);

		return services;

	}

}
