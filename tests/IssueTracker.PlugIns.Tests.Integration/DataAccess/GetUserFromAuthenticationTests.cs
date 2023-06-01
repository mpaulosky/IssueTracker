namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUserFromAuthenticationTests : IAsyncLifetime
{
	private const string CleanupValue = "users";
	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;

	public GetUserFromAuthenticationTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact]
	public async Task GetFromAuthenticationAsync_With_ValidData_Should_ReturnAUser_Test()
	{
		// Arrange
		var expected = FakeUser.GetNewUser();
		await _sut.CreateAsync(expected);

		// Act
		var result = await _sut.GetFromAuthenticationAsync(expected.ObjectIdentifier);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}
}
