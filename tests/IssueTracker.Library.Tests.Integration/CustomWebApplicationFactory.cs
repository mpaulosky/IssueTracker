using IssueTracker.Library.DataAccess;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using TestingSupport.Library.Fixtures;

namespace IssueTracker.Library;

public class CustomWebApplicationFactory<TStartup>
		: WebApplicationFactory<TStartup> where TStartup : class
{

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{

		var databaseName = $"test_db_{Guid.NewGuid()}";
		const string databaseConnectionString = "mongodb://MongoAdmin:test123@localhost:27017/?authMechanism=DEFAULT&authSource=admin";

		builder.ConfigureServices(services =>
		{

			var descriptor = services.SingleOrDefault(predicate: d => d.ServiceType == typeof(DatabaseSettings));

			services.Remove(descriptor!);

			services.Configure<DatabaseSettings>((IConfiguration)TestFixtures.Settings(databaseName!, databaseConnectionString));

			var sp = services.BuildServiceProvider();

			using var scope = sp.CreateScope();

			try
			{
				//Utilities.InitializeDbForTests(db);
			}
			catch (Exception)
			{
			}

		});

	}

}
