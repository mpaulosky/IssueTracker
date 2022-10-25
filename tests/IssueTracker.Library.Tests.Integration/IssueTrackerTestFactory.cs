using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

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

		builder.ConfigureTestServices(services =>
		{

			services.RemoveAll(typeof(IServiceCollection));

			services.RemoveAll(typeof(IMongoDbContextFactory));

			services.AddSingleton<IMongoDbContextFactory>(_ =>
					new MongoDbContextFactory(_dbFixture.DbConfig.ConnectionString, _dbFixture.DbConfig.DatabaseName));

		});

	}

}