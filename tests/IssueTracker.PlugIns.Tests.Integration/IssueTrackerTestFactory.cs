// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IssueTrackerTestFactory.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Testcontainers.MongoDb;

namespace IssueTracker.PlugIns;

[ExcludeFromCodeCoverage]
[UsedImplicitly]
public class IssueTrackerTestFactory : WebApplicationFactory<IAppMarker>, IAsyncLifetime
{
	private readonly ILogger<IssueTrackerTestFactory> _logger;
	private readonly string _databaseName;
	private readonly CancellationTokenSource _cts;
	private static MongoDbContainer? _sharedContainer;
	private static readonly Lock Lock = new();
	private static readonly SemaphoreSlim DbLock = new(1, 1);

	public IssueTrackerTestFactory()
	{
		_databaseName = $"test_db_{Guid.NewGuid()}";
		_cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

		var loggerFactory = LoggerFactory.Create(builder =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Debug);
		});
		_logger = loggerFactory.CreateLogger<IssueTrackerTestFactory>();

		// Initialize a shared container if not already done
		if (_sharedContainer == null)
		{
			lock (Lock)
			{
				if (_sharedContainer == null)
				{
					_sharedContainer = new MongoDbBuilder()
						.WithImage("mongo:8.0")
						.WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "admin")
						.WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "password")
						.Build();
				}
			}
		}
	}

	private string GetConnectionString()
	{
		var port = _sharedContainer!.GetMappedPublicPort(27017);
		return $"mongodb://admin:password@localhost:{port}/{_databaseName}?authSource=admin";
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		// Set Redis as disabled via environment variable BEFORE base.ConfigureWebHost()
		// so ServiceDefaults reads it correctly when AddServiceDefaults() is called
		Environment.SetEnvironmentVariable("Redis__Enabled", "false");

		base.ConfigureWebHost(builder);

		builder.ConfigureAppConfiguration((context, config) =>
		{
			var connectionString = GetConnectionString();

			config.AddInMemoryCollection(new Dictionary<string, string?>
			{
				["MongoDbSettings:ConnectionStrings"] = connectionString,
				["MongoDbSettings:DatabaseName"] = _databaseName
			});
		});

		builder.ConfigureServices(services =>
		{
			var connectionString = GetConnectionString();

			// Register IDatabaseSettings
			services.AddSingleton<IDatabaseSettings>(new DatabaseSettings(connectionString, _databaseName));

			// Register IMongoClient
			services.AddSingleton<IMongoClient>(sp =>
			{
				_logger.LogInformation("Using MongoDB connection string: {ConnectionString}", connectionString);
				return new MongoClient(connectionString);
			});
		});
	}

	public async Task InitializeAsync()
	{
		try
		{
			_logger.LogInformation("Starting MongoDB container...");
			await _sharedContainer!.StartAsync(_cts.Token);
			_logger.LogInformation("MongoDB container started successfully");

			// Wait for MongoDB to be ready
			var port = _sharedContainer.GetMappedPublicPort(27017);
			var client = new MongoClient($"mongodb://admin:password@localhost:{port}/?authSource=admin");
			var maxRetries = 30;
			var retryDelayMs = 1000;
			for (int i = 0; i < maxRetries; i++)
			{
				try
				{
					await (await client.ListDatabaseNamesAsync()).FirstOrDefaultAsync(_cts.Token);
					_logger.LogInformation("Successfully connected to MongoDB");
					break;
				}
				catch (Exception ex)
				{
					_logger.LogWarning(ex, "Failed to connect to MongoDB (attempt {Attempt}/{MaxRetries})", i + 1, maxRetries);
					if (i < maxRetries - 1)
					{
						await Task.Delay(retryDelayMs, _cts.Token);
					}
				}
			}
		}
		catch (OperationCanceledException)
		{
			_logger.LogError("MongoDB container startup timed out after 5 minutes");
			throw;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to start MongoDB container");
			throw;
		}
	}

	public override async ValueTask DisposeAsync()
	{
		try
		{
			// Clean up the database for this test instance
			try
			{
				var client = Services.GetRequiredService<IMongoClient>();
				await client.DropDatabaseAsync(_databaseName);
				_logger.LogInformation("Database {DatabaseName} dropped successfully", _databaseName);
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Error dropping database {DatabaseName}", _databaseName);
			}

			// Note: We don't dispose the shared container here because it's shared across all tests
			// The container will be cleaned up when the process exits
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in DisposeAsync");
		}
		finally
		{
			_cts.Dispose();
			await base.DisposeAsync();
		}
	}

	public async Task ResetDatabaseAsync()
	{
		try
		{
			await DbLock.WaitAsync();
			var client = Services.GetRequiredService<IMongoClient>();
			await client.DropDatabaseAsync(_databaseName);
			_logger.LogInformation("Database {DatabaseName} dropped successfully", _databaseName);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error dropping database");
			throw;
		}
		finally
		{
			DbLock.Release();
		}
	}

	public async Task ResetCollectionAsync(string collectionName)
	{
		try
		{
			await DbLock.WaitAsync();
			var client = Services.GetRequiredService<IMongoClient>();
			var database = client.GetDatabase(_databaseName);
			await database.DropCollectionAsync(collectionName);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error resetting collection {CollectionName}", collectionName);
			throw;
		}
		finally
		{
			DbLock.Release();
		}
	}

	Task IAsyncLifetime.DisposeAsync()
	{
		return DisposeAsync().AsTask();
	}
}
