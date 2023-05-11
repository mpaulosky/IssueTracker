namespace IssueTracker.PlugIns.Tests.Integration.MongoContainerTests;

[Collection("Test Collection")]
[ExcludeFromCodeCoverage]
public class MongoDbContainerTest : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private const string? CleanupValue = "";

	public MongoDbContainerTest(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		_factory.DbContext = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
	}

	[Fact]
	public void ConnectionStateReturnsOpen()
	{
		// Given
		var client = _factory.DbContext.Client;

		// When
		using var databases = client.ListDatabases();

		// Then
		Assert.Contains(databases.ToEnumerable(), database => database.TryGetValue("name", out var name) && "admin".Equals(name.AsString));
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

}
