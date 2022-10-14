
namespace IssueTracker.UI.Tests.Integration;

public class IssueTrackerUIFactory : WebApplicationFactory<IAppMarker>, IAsyncLifetime
{

	private readonly TestcontainerDatabase _dbContainer =
			new TestcontainersBuilder<MongoDbTestcontainer>()
					.WithDatabase(new MongoDbTestcontainerConfiguration
					{

						Database = "db",
						Username = "course",
						Password = "whatever"

					}).Build();


	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		var descriptorDatabaseSettings = builder.ConfigureServices(services =>
		{

			var descriptorMongoDbContext = services.FirstOrDefault(d => d.ServiceType == typeof(MongoDbContext));
			services.Remove(item: descriptorMongoDbContext!);

			//var dbSettings = new DatabaseSettings("mongodb://course:whatever@localhost:27017/?authSource=admin", DbName);
			var dbSettings = new DatabaseSettings(_dbContainer.ConnectionString, _dbContainer.Database);

			services.AddSingleton<IDatabaseSettings>(dbSettings);
			services.AddSingleton<IMongoDbContextFactory, TestContextFactory>();
			services.AddSingleton<ICategoryService, CategoryService>();
			services.AddSingleton<ICommentService, CommentService>();
			services.AddSingleton<IStatusService, StatusService>();
			services.AddSingleton<IIssueService, IssueService>();
			services.AddSingleton<IUserService, UserService>();

			services.AddSingleton<ICategoryRepository, CategoryRepository>();
			services.AddSingleton<ICommentRepository, CommentRepository>();
			services.AddSingleton<IStatusRepository, StatusRepository>();
			services.AddSingleton<IIssueRepository, IssueRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();

		});

	}

	public async Task InitializeAsync()
	{

		await _dbContainer.StartAsync();

	}

	public new async Task DisposeAsync()
	{

		await _dbContainer.StopAsync();
		await _dbContainer.DisposeAsync();

	}

}
