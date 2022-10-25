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
		
		Console.WriteLine($@"DbFixture DbContextSettings: {DbConfig.ConnectionString}, {DbConfig.DatabaseName}");

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

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DbFixture>
{

// This class has no code, and is never created. Its purpose is simply
// to be the place to apply [CollectionDefinition] and all the
// ICollectionFixture<> interfaces.

}