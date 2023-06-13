// Copyright (c) 2023. All rights reserved.
// File Name :     GetStatusTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetStatusTests : IAsyncLifetime
{
	private const string CleanupValue = "statuses";

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;

	public GetStatusTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		IMongoDbContextFactory context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
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
	public async Task GetAsync_With_WithData_Should_ReturnAValidStatus_TestAsync()
	{
		// Arrange
		StatusModel expected = FakeStatus.GetNewStatus();
		await _sut.CreateAsync(expected);

		// Act
		StatusModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);
	}

	[Theory(DisplayName = "GetAsync Without Valid Data Should Fail")]
	[InlineData("62cf2ad6326e99d665759e5a")]
	public async Task GetAsync_With_WithoutData_Should_ReturnNothing_TestAsync(string? value)
	{
		// Arrange

		// Act
		StatusModel result = await _sut.GetAsync(value!);

		// Assert
		result.Should().BeNull();
	}
}