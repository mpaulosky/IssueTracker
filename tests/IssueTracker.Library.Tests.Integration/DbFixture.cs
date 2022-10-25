namespace IssueTracker.Library;

[ExcludeFromCodeCoverage]
public class DbFixture : IDisposable
{
	private TestContextFactory DbContext { get; }
	public DatabaseSettings dbConfig { get; }
	private IConfiguration AppConfiguration { get; }

	public DbFixture()
	{

		AppConfiguration = LoadConfig("appsettings-integration-tests.json");
		
		dbConfig = AppConfiguration.GetSection("MongoDbSettings").Get<DatabaseSettings>();

		dbConfig.DatabaseName = "test_" + Guid.NewGuid().ToString("N");
		
		Console.WriteLine($@"DbFixture DbContextSettings: {dbConfig.ConnectionString}, {dbConfig.DatabaseName}");

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

		//DbContext.Client.DropDatabase(dbConfig.DatabaseName);

	}

}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DbFixture>
{

// This class has no code, and is never created. Its purpose is simply
// to be the place to apply [CollectionDefinition] and all the
// ICollectionFixture<> interfaces.

}