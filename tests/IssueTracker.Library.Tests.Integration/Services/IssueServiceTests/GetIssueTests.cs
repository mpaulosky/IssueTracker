namespace IssueTracker.Library.Services.IssueServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetIssueTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;
	private string _cleanupValue;

	public GetIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task GetIssue_With_Data_Should_ReturnAValidIssue_TestAsync()
	{

		// Arrange
		_cleanupValue = "issues";
		var expected = FakeIssue.GetNewIssue();
		await _sut.CreateIssue(expected);

		// Act
		var result = await _sut.GetIssue(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.IssueName.Should().Be(expected.IssueName);
		result.Id.Should().Be(expected.Id);
		result.Author.Id.Should().Be(expected.Author.Id);

	}

	[Fact]
	public async Task GetIssue_WithOutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		_cleanupValue = "";
		var id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetIssue(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetIssue_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		_cleanupValue = "";
		string id = null;

		// Act
		var act = async () => await _sut.GetIssue(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

	[Fact]
	public async Task GetIssue_With_EmptyStringId_Should_ThrowArgumentException_Test()
	{

		// Arrange
		_cleanupValue = "";
		var id = "";

		// Act
		var act = async () => await _sut.GetIssue(id);

		// Assert
		await act.Should().ThrowAsync<ArgumentException>();

	}

	public Task InitializeAsync() => Task.CompletedTask;

	public async Task DisposeAsync()
	{

		await _factory.ResetDatabaseAsync(_cleanupValue);

	}

}