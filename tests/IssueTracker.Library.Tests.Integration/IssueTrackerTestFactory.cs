namespace IssueTracker.PlugIns;

[Collection("Test collection")]
[ExcludeFromCodeCoverage]
public class IssueTrackerTestFactory : WebApplicationFactory<IAppMarker>, IAsyncLifetime
{

	private readonly MongoDbContainer _mongoDbContainer;
	private readonly string _databaseName;

	public IConfiguration AppConfiguration { get; }

	public DatabaseSettings DbConfig { get; set; }

	public IMongoDbContextFactory DbContext { get; set; }

	public IssueTrackerTestFactory()
	{

		_mongoDbContainer = new MongoDbBuilder().Build();

		_databaseName = $"test_{Guid.NewGuid():N}";

	}
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		builder.ConfigureTestServices(services =>
		{

			var dbConnectionDescriptor = services.SingleOrDefault(
					d => d.ServiceType ==
							typeof(IMongoDbContextFactory));

			services.Remove(dbConnectionDescriptor);

			services.AddSingleton<IMongoDbContextFactory>(_ =>
					new MongoDbContextFactory(DbConfig));

			using var serviceProvider = services.BuildServiceProvider();

			DbContext = serviceProvider.GetRequiredService<IMongoDbContextFactory>();

		});

		builder.UseEnvironment("Development");

	}

	public async Task ResetCollectionAsync(string collection)
	{

		if (!string.IsNullOrWhiteSpace(collection))
		{

			await DbContext.Client.GetDatabase(_databaseName).DropCollectionAsync(collection);

		}

	}

	public async Task InitializeAsync()
	{

		await _mongoDbContainer.StartAsync();

		DbConfig = new DatabaseSettings(_mongoDbContainer.GetConnectionString(), _databaseName);

	}

	public new async Task DisposeAsync()
	{

		await DbContext.Client.DropDatabaseAsync(_databaseName);
		await _mongoDbContainer.DisposeAsync().ConfigureAwait(false);

	}

}
