﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetStatusTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;
	private const string CleanupValue = "statuses";

	public GetStatusTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new StatusRepository(context);

	}

	[Fact]
	public async Task GetAsync_With_WithData_Should_ReturnAValidStatus_TestAsync()
	{

		// Arrange
		var expected = FakeStatus.GetNewStatus();
		await _sut.CreateAsync(expected);

		// Act
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Theory(DisplayName = "GetAsync Without Valid Data Should Fail")]
	[InlineData("62cf2ad6326e99d665759e5a")]
	public async Task GetAsync_With_WithoutData_Should_ReturnNothing_TestAsync(string? value)
	{
		// Arrange

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

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
