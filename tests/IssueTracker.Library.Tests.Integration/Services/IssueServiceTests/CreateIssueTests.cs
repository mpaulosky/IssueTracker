namespace IssueTracker.Library.Services.StatusServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class CreateIssueTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;

	public CreateIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task CreateIssue_With_ValidData_Should_CreateAIssue_TestAsync()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();

		// Act
		await _sut.CreateIssue(expected);

		// Assert
		expected.Id.Should().NotBeNull();

	}

	[Fact]
	public async Task CreateIssue_With_InValidData_Should_FailToCreateAIssue_TestAsync()
	{

		// Arrange
		IssueModel expected = null;

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateIssue(expected));

	}

}