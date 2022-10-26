using Microsoft.Extensions.DependencyInjection.Extensions;

using static System.Net.Mime.MediaTypeNames;

namespace IssueTracker.Library;

[Collection("Database")]
[ExcludeFromCodeCoverage]
public class IssueTrackerTestFactory : WebApplicationFactory<IAppMarker>, IDisposable
{
	private readonly DbFixture _dbFixture;
	private IMongoDbContextFactory DbContext { get; set; }

	public IssueTrackerTestFactory(DbFixture fixture)
	{

		_dbFixture = fixture;

	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		builder.ConfigureTestServices(services =>
		{

			services.RemoveAll(typeof(IServiceCollection));

			services.RemoveAll(typeof(IMongoDbContextFactory));

			services.AddSingleton<IMongoDbContextFactory>(_ =>
					new MongoDbContextFactory(_dbFixture.DbConfig.ConnectionString, _dbFixture.DbConfig.DatabaseName));

			using var serviceProvider = services.BuildServiceProvider();
			DbContext = serviceProvider.GetRequiredService<IMongoDbContextFactory>();

		});

	}

	public new void Dispose()
	{
		DbContext.Client.DropDatabase(_dbFixture.DbConfig.DatabaseName);
	}

}