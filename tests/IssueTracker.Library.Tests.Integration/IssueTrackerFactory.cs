using DotNet.Testcontainers.Builders;

using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using IssueTracker.Library.Contracts;
using IssueTracker.Library.DataAccess;
using IssueTracker.Library.Helpers;
using IssueTracker.Library.Services;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

using Xunit;

namespace IssueTracker.Library;

public class IssueTrackerFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
	private readonly TestcontainerDatabase _dbContainer =
				new TestcontainersBuilder<MongoDbTestcontainer>()
						.WithDatabase(
							new MongoDbTestcontainerConfiguration { Database = "dbTest", Username = "admin", Password = "whatever" })
		.Build();

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

				var newDatabaseSettings = new DatabaseSettings()
				{
					DatabaseName = "dbTest",
					ConnectionString = _dbContainer.ConnectionString
				};

				services.Configure<DatabaseSettings>((Microsoft.Extensions.Configuration.IConfiguration)newDatabaseSettings);

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

	
	public async Task InitializeAsync() { await _dbContainer.StartAsync(); }

	public new async Task DisposeAsync() { await _dbContainer.DisposeAsync(); }
}