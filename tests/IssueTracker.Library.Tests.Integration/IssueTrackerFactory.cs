namespace IssueTracker.Library;

public class IssueTrackerFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
	private readonly TestcontainerDatabase _dbContainer =
		new TestcontainersBuilder<MongoDbTestcontainer>()
			.WithDatabase(
				new MongoDbTestcontainerConfiguration
				{
					Database = "dbTest", Username = "admin", Password = "whatever"
				})
			.Build();


	public async Task InitializeAsync() { await _dbContainer.StartAsync(); }

	public new async Task DisposeAsync() { await _dbContainer.DisposeAsync(); }

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureLogging(
			logging =>
			{
				//logging.ClearProviders();
			});

		builder.ConfigureTestServices(
			services =>
			{
				services.RemoveAll(typeof(IHostedService));

				services.RemoveAll(typeof(IDbConnectionFactory));

				DatabaseSettings newDatabaseSettings = new DatabaseSettings
				{
					DatabaseName = "dbTest", ConnectionString = _dbContainer.ConnectionString
				};

				services.Configure<DatabaseSettings>((IConfiguration)newDatabaseSettings);

				services.AddSingleton<IMongoDbContextFactory, TestConnectionFactory>();
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
}