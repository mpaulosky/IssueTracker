
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

		builder.ConfigureServices(services =>
		{

			services.Configure<DatabaseSettings>(config.GetSection("MongoDbSettings"));

			services.AddHealthChecks()
			.AddMongoDb(
				mongodbConnectionString: config.GetValue<string>("MongoDbSettings:ConnectionStrings"), 
				mongoDatabaseName: config.GetValue<string>("MongoDbSettings:DatabaseName"),
				name: "testMongodb");

			services.AddSingleton<IDatabaseSettings>(_dbFixture.DbContextSettings);
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

}