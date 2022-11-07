using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using Microsoft.VisualBasic;

namespace IssueTracker.Library;

[ExcludeFromCodeCoverage]
public class IssueTrackerUIFactory : WebApplicationFactory<IAppMarker>, IAsyncLifetime
{

	private readonly MongoDbTestcontainer _dbContainer;

	public IssueTrackerUIFactory()
	{

		_dbContainer = new TestcontainersBuilder<MongoDbTestcontainer>()
			.WithDatabase(new MongoDbTestcontainerConfiguration
				{
					Database = "testdb", 
					Username = "test", 
					Password = "whatever"
				})
			.WithImage("mongo")
			.WithName("testdb")
			.Build();
	}

	private MongoDbContextFactory _dbConnection = default!;

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(services =>
		{
			services.RemoveAll(typeof(IMongoDbContextFactory));
			services.AddSingleton<IMongoDbContextFactory>(_ =>
				new MongoDbContextFactory(_dbContainer.ConnectionString, _dbContainer.Database));
		});
	}

	public async Task ResetDatabaseAsync(string collection)
	{
		await _dbConnection.Database.DropCollectionAsync(collection);
	}

	public async Task InitializeAsync()
	{
		await _dbContainer.StartAsync();
		_dbConnection = new MongoDbContextFactory(_dbContainer.ConnectionString, _dbContainer.Database);
		await InitializeRespawner();
	}

	private async Task InitializeRespawner()
	{
		await _dbConnection.Client.DropDatabaseAsync(_dbContainer.Database);
	}

	public new async Task DisposeAsync()
	{
		await _dbContainer.StopAsync();
	}
}