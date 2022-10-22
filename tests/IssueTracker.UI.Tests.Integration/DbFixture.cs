using IssueTracker.Library;

using Microsoft.Extensions.Configuration;

namespace IssueTracker.Library
{
	public class DbFixture : IDisposable
	{
		public DatabaseSettings DbContextSettings { get; }
		public TestContextFactory DbContext { get; }
		public string DbName { get; }

		public DbFixture()
		{

			var config = new ConfigurationBuilder()
					.AddJsonFile("appsettings.json")
					.Build();

			var connString = config.GetValue<string>("MongoDbSettings:ConnectionStrings");
			DbName = $"test_db_{Guid.NewGuid()}";

			DbContextSettings = new(connString, DbName);

			DbContext = new TestContextFactory(DbContextSettings);
		
		}

		public void Dispose()
		{

			DbContext.Client.DropDatabase(DbName);

		}

	}

}

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DbFixture>
{
	// This class has no code, and is never created. Its purpose is simply
	// to be the place to apply [CollectionDefinition] and all the
	// ICollectionFixture<> interfaces.
}