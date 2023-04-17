namespace IssueTracker.PlugIns.MongoContainerTests;

[Collection("Test Collection")]
[ExcludeFromCodeCoverage]
public class MongoDbContainerTest : IAsyncLifetime
{
	private readonly IssueTrackerTestFactory _factory;
	private readonly string _cleanupValue = string.Empty;

	public MongoDbContainerTest(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		_factory.DbContext = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(_cleanupValue);
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

}
