﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetStatusesTests : IAsyncLifetime
{
	private const string CleanupValue = "statuses";

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;

	public GetStatusesTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);
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
	public async Task GetAllAsync_With_ValidData_Should_ReturnStatuses_Test()
	{
		// Arrange
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateAsync(expected);

		// Act	
		var results = (await _sut.GetAllAsync()).ToList();

		// Assert
		results.Count.Should().Be(1);
		results.First().StatusName.Should().Be(expected.StatusName);
		results.First().StatusDescription.Should().Be(expected.StatusDescription);
	}
}
