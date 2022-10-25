namespace IssueTracker.Library;

[Collection("Database")]
[ExcludeFromCodeCoverage]
public class IssueTrackerTestFactory : WebApplicationFactory<IAppMarker>
{
	private readonly DbFixture _dbFixture;

	public IssueTrackerTestFactory(DbFixture fixture)
	{
		_dbFixture = fixture;
	}

	// protected override void ConfigureWebHost(IWebHostBuilder builder)
	// {
	//
	// 	var config = new ConfigurationBuilder().AddJsonFile("appsettings-integration-tests.json").Build();
	//
	// 	builder.ConfigureServices(services =>
	// 	{
	//
	//
	// 		services.Configure<DatabaseSettings>(config.GetSection("MongoDbSettings"));
	//
	// 		Console.WriteLine($@"DbFixture DbContextSettings: {_dbFixture.DbContextSettings.ConnectionString}, {_dbFixture.DbContextSettings.DatabaseName}");
	//
	// 		services.AddHealthChecks()
	// 		.AddMongoDb(
	// 			mongodbConnectionString: _dbFixture.DbContextSettings.ConnectionString,
	// 			mongoDatabaseName: _dbFixture.DbContextSettings.DatabaseName,
	// 			name: "testMongodb");
	//
	// 		services.AddSingleton<IDatabaseSettings>(_dbFixture.DbContextSettings);
	// 		services.AddSingleton<IMongoDbContextFactory, TestContextFactory>();
	// 		services.AddSingleton<ICategoryService, CategoryService>();
	// 		services.AddSingleton<ICommentService, CommentService>();
	// 		services.AddSingleton<IStatusService, StatusService>();
	// 		services.AddSingleton<IIssueService, IssueService>();
	// 		services.AddSingleton<IUserService, UserService>();
	//
	// 		services.AddSingleton<ICategoryRepository, CategoryRepository>();
	// 		services.AddSingleton<ICommentRepository, CommentRepository>();
	// 		services.AddSingleton<IStatusRepository, StatusRepository>();
	// 		services.AddSingleton<IIssueRepository, IssueRepository>();
	// 		services.AddSingleton<IUserRepository, UserRepository>();
	//
	// 	});
	//
	// }

}