using IssueTracker.UI;

namespace IssueTracker.Library;

[Collection("Database")]
public class IssueTrackerTestFactory : WebApplicationFactory<IAppMarker>
{
	private readonly DbFixture _dbFixture;

	public IssueTrackerTestFactory(DbFixture fixture)
	{
		_dbFixture = fixture;
	}


	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		builder.ConfigureServices(services =>
		{

			var descriptorMongoDbContext = services.FirstOrDefault(d => d.ServiceType == typeof(MongoDbContextFactory));
			services.Remove(item: descriptorMongoDbContext);

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