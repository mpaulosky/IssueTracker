using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace IssueTracker.Library;

[ExcludeFromCodeCoverage]
internal class IssueTrackerUIFactory : WebApplicationFactory<IAppMarker>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		builder.ConfigureTestServices(services =>
		{
			services.RemoveAll(typeof(IHostedService));

			services.RemoveAll(typeof(IMongoDbContextFactory));

			//services.AddSingleton<IMongoDbContextFactory>(_ =>
			//		new MongoDbContextFactory(_dbContainer.ConnectionString));

		});
	}
}