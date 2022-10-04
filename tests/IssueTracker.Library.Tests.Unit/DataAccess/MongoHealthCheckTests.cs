using IssueTracker.Library.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using NSubstitute;

using System.Net;

namespace IssueTracker.Library.DataAccess;

public class MongoHealthCheckTests
{

	[Fact]
	public void add_named_health_check_when_properly_configured_connectionString()
	{
		var services = new ServiceCollection();
		services.AddHealthChecks()
				.AddMongoDb("mongodb://connectionstring", "mongodb");

		using var serviceProvider = services.BuildServiceProvider();
		var options = serviceProvider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();

		var registration = options.Value.Registrations.First();
		var check = registration.Factory(serviceProvider);

		registration.Name.Should().Be("mongodb");
		check.GetType().Should().Be(typeof(MongoDbHealthCheck));
	}

}

public class mongodb_healthcheck_should
{
	private readonly string[] args;

	[Fact]
	public async Task be_healthy_listing_all_databases_if_mongodb_is_available()
	{
		var connectionString = @"mongodb://localhost:27017";

		var builder = WebApplication.CreateBuilder(args); ;

		var services = new ServiceCollection();

		builder.Services.AddHealthChecks()
						.AddMongoDb(connectionString, "testDb", tags: new string[] { "mongodb" });

		var app = builder.Build();

		app.UseHealthChecks("/health", new HealthCheckOptions
		{
			Predicate = r => r.Tags.Contains("mongodb")
		});

		using var server = new TestServer(builder);

		var response = await server.CreateRequest("/health")
				.GetAsync();

		response.StatusCode
				.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task be_healthy_on_specified_database_if_mongodb_is_available_and_database_exist()
	{
		var connectionString = @"mongodb://localhost:27017";

		var webHostBuilder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					services.AddHealthChecks()
									.AddMongoDb(connectionString, mongoDatabaseName: "local", tags: new string[] { "mongodb" });
				})
				.Configure(app =>
				{
					app.UseHealthChecks("/health", new HealthCheckOptions
					{
						Predicate = r => r.Tags.Contains("mongodb")
					});
				});

		using var server = new TestServer(webHostBuilder);

		var response = await server.CreateRequest("/health")
				.GetAsync();

		response.StatusCode
				.Should().Be(HttpStatusCode.OK);
	}
	[Fact]
	public async Task be_healthy_on_connectionstring_specified_database_if_mongodb_is_available_and_database_exist()
	{
		var connectionString = @"mongodb://localhost:27017/local";

		var webHostBuilder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					services.AddHealthChecks()
									.AddMongoDb(connectionString, tags: new string[] { "mongodb" });
				})
				.Configure(app =>
				{
					app.UseHealthChecks("/health", new HealthCheckOptions
					{
						Predicate = r => r.Tags.Contains("mongodb")
					});
				});

		using var server = new TestServer(webHostBuilder);

		var response = await server.CreateRequest("/health")
				.GetAsync();

		response.StatusCode
				.Should().Be(HttpStatusCode.OK);
	}
	[Fact]
	public async Task be_unhealthy_on_connectionstring_specified_database_if_mongodb_is_available_and_database_exist()
	{
		var connectionString = @"mongodb://localhost:27017/nonexisting";

		var webHostBuilder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					services.AddHealthChecks()
									.AddMongoDb(connectionString, tags: new string[] { "mongodb" });
				})
				.Configure(app =>
				{
					app.UseHealthChecks("/health", new HealthCheckOptions
					{
						Predicate = r => r.Tags.Contains("mongodb")
					});
				});

		using var server = new TestServer(webHostBuilder);

		var response = await server.CreateRequest("/health")
				.GetAsync();

		response.StatusCode
				.Should().Be(HttpStatusCode.ServiceUnavailable);
	}

	[Fact]
	public async Task be_unhealthy_listing_all_databases_if_mongodb_is_not_available()
	{
		var webHostBuilder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					services.AddHealthChecks()
									.AddMongoDb("mongodb://nonexistingdomain:27017", tags: new string[] { "mongodb" });
				})
				.Configure(app =>
				{
					app.UseHealthChecks("/health", new HealthCheckOptions
					{
						Predicate = r => r.Tags.Contains("mongodb")
					});
				});

		using var server = new TestServer(webHostBuilder);

		var response = await server.CreateRequest("/health")
				.GetAsync();

		response.StatusCode
				.Should().Be(HttpStatusCode.ServiceUnavailable);
	}

	[Fact]
	public async Task be_unhealthy_on_specified_database_if_mongodb_is_not_available()
	{
		var webHostBuilder = new WebHostBuilder()
				.ConfigureServices(services =>
				{
					services.AddHealthChecks()
									.AddMongoDb("mongodb://nonexistingdomain:27017", tags: new string[] { "mongodb" });
				})
				.Configure(app =>
				{
					app.UseHealthChecks("/health", new HealthCheckOptions
					{
						Predicate = r => r.Tags.Contains("mongodb")
					});
				});

		using var server = new TestServer(webHostBuilder);

		var response = await server.CreateRequest("/health")
				.GetAsync();

		response.StatusCode
				.Should().Be(HttpStatusCode.ServiceUnavailable);
	}
}