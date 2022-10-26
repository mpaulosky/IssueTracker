namespace IssueTracker.Library.Services.IssueServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class UpdateIssueTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly IssueService _sut;

	public UpdateIssueTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var repo = (IIssueRepository)_factory.Services.GetRequiredService(typeof(IIssueRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new IssueService(repo, memCache);

	}

	[Fact]
	public async Task UpdateIssue_With_ValidData_Should_UpdateTheIssue_Test()
	{

		// Arrange
		var expected = FakeIssue.GetNewIssue();
		await _sut.CreateIssue(expected);

		// Act
		expected.Description = "Updated";
		await _sut.UpdateIssue(expected);
		var result = await _sut.GetIssue(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Description.Should().Be(expected.Description);

	}

	[Fact]
	public async Task UpdateIssue_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{

		// Arrange
		IssueModel expected = null;

		// Act
		var act = async () => await _sut.UpdateIssue(expected);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

	}

}
