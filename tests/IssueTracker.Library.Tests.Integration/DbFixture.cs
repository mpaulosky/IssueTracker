namespace IssueTracker.Library;

[ExcludeFromCodeCoverage]
public class DbFixture : IDisposable
{

	public DatabaseSettings DbConfig { get; }
	private IConfiguration AppConfiguration { get; }

	public DbFixture()
	{

		AppConfiguration = LoadConfig("appsettings-integration-tests.json");
		
		DbConfig = AppConfiguration.GetSection("MongoDbSettings").Get<DatabaseSettings>();

		DbConfig.DatabaseName = "test_" + Guid.NewGuid().ToString("N");

	}

	private static IConfiguration LoadConfig(string appSettings)
	{

		var config = new ConfigurationBuilder()
			.AddJsonFile(appSettings, optional: false, false)
			.AddEnvironmentVariables()
			.Build();

		return config;

	}

	public void Dispose()
	{

	}

}