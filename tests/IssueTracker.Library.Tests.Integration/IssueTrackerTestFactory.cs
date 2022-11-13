namespace IssueTracker.Library;

[Collection("Test collection")]
[ExcludeFromCodeCoverage]

public class IssueTrackerTestFactory : WebApplicationFactory<IAppMarker>, IAsyncLifetime
{

	public DatabaseSettings DbConfig { get; }
	private IConfiguration AppConfiguration { get; }

	private IMongoDbContextFactory DbContext { get; set; }

	public IssueTrackerTestFactory()
	{

		AppConfiguration = LoadConfig("appsettings-integration-tests.json");

		DbConfig = AppConfiguration.GetSection("MongoDbSettings").Get<DatabaseSettings>();

		DbConfig.DatabaseName = "test_" + Guid.NewGuid().ToString("N");

	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		builder.ConfigureTestServices(services =>
		{

			services.RemoveAll(typeof(IServiceCollection));

			services.RemoveAll(typeof(IMongoDbContextFactory));

			services.AddSingleton<IMongoDbContextFactory>(_ =>
					new MongoDbContextFactory(DbConfig.ConnectionString, DbConfig.DatabaseName));

			using var serviceProvider = services.BuildServiceProvider();

			DbContext = serviceProvider.GetRequiredService<IMongoDbContextFactory>();

		});

	}

	public async Task ResetDatabaseAsync(string collection)
	{

		if (string.IsNullOrWhiteSpace(collection) == false)
		{

			await DbContext.Database.DropCollectionAsync(collection);

		}

	}

	private static IConfiguration LoadConfig(string appSettings)
	{

		var config = new ConfigurationBuilder()
			.AddJsonFile(appSettings, optional: false, false)
			.AddEnvironmentVariables()
			.Build();

		return config;

	}

	public async Task InitializeAsync()
	{

		await Task.Delay(1000);

	}

	public new async Task DisposeAsync()
	{

		await DbContext.Client.DropDatabaseAsync(DbConfig.DatabaseName);

	}

}