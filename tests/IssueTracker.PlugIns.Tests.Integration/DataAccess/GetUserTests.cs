namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetUserTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly UserRepository _sut;
	private string? _cleanupValue;

	public GetUserTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new UserRepository(context);

	}

	[Fact]
	public async Task GetAsync_With_WithData_Should_ReturnAValidUser_TestAsync()
	{

		// Arrange
		_cleanupValue = "users";
		var expected = FakeUser.GetNewUser();
		await _sut.CreateAsync(expected);

		// Act
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Theory]
	[InlineData("62cf2ad6326e99d665759e5a")]
	public async Task GetAsync_With_WithoutData_Should_ReturnNothing_TestAsync(string? value)
	{
		// Arrange
		_cleanupValue = "";

		// Act
		var result = await _sut.GetAsync(value!);

		// Assert
		result.Should().BeNull();

	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(_cleanupValue);

	}

}
