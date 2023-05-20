namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateIssueTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueRepository _sut;
	private const string CleanupValue = "issues";

	public UpdateIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new IssueRepository(context);

	}

	[Fact]
	public async Task UpdateAsync_With_ValidData_Should_UpdateTheIssue_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		await _sut.CreateAsync(expected);

		// Act
		expected.Description = "Updated";
		await _sut.UpdateAsync(expected.Id, expected);
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Description.Should().Be(expected.Description);

	}

	[Fact]
	public async Task UpdateAsync_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange

		// Act
		Func<Task> act = async () => await _sut.UpdateAsync(null!, null!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

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
