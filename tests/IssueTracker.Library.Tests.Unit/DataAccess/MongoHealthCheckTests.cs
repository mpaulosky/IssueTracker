namespace IssueTracker.Library.DataAccess;

[ExcludeFromCodeCoverage]
public class MongoHealthCheckTests
{

	[Fact]
	public void Add_named_health_check_when_properly_configured_connectionString()
	{
		// Arrange
		const string connectionString = "mongodb://connectionstring";
		const string databaseName = "mongodb";

		var services = new ServiceCollection();
		services.AddHealthChecks()
				.AddMongoDb(connectionString, databaseName);

		using ServiceProvider serviceProvider = services.BuildServiceProvider();
		IOptions<HealthCheckServiceOptions> options = serviceProvider.GetRequiredService<IOptions<HealthCheckServiceOptions>>();

		// Act
		HealthCheckRegistration registration = options.Value.Registrations.First();
		IHealthCheck check = registration.Factory(serviceProvider);

		// Assert
		registration.Name.Should().Be("mongodb");
		check.GetType().Should().Be(typeof(MongoDbHealthCheck));

	}

	[Fact]
	public void Add_named_health_check_when_empty_connectionString_should_fail()
	{

		// Arrange
		const string connectionString = "";
		const string databaseName = "mongodb";

		var services = new ServiceCollection();

		// Assert
		Assert.Throws<ArgumentException>(() => services.AddHealthChecks().AddMongoDb(connectionString, databaseName));

	}

	[Fact]
	public void Add_named_health_check_when_null_connectionString_should_fail()
	{

		// Arrange
		const string connectionString = null;
		const string databaseName = "mongodb";

		var services = new ServiceCollection();

		// Assert
		Assert.Throws<ArgumentNullException>(() => services.AddHealthChecks().AddMongoDb(connectionString!, databaseName));

	}

	[Fact]
	public void Add_named_health_check_when_empty_databaseName_should_fail()
	{

		// Arrange
		const string connectionString = "mongodb://connectionstring";
		const string databaseName = "";

		var services = new ServiceCollection();

		// Assert
		Assert.Throws<ArgumentException>(() => services.AddHealthChecks().AddMongoDb(connectionString, databaseName));

	}

	[Fact]
	public void Add_named_health_check_when_null_databaseName_should_fail()
	{

		// Arrange
		const string connectionString = "mongodb://connectionstring";
		const string databaseName = null;

		var services = new ServiceCollection();

		// Assert
		Assert.Throws<ArgumentNullException>(() => services.AddHealthChecks().AddMongoDb(connectionString, databaseName!));

	}

}