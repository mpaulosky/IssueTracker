// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     UpdateStatusTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateStatusTests : IAsyncLifetime
{
	private const string CleanupValue = "statuses";

	private readonly IssueTrackerTestFactory _factory;
	private readonly StatusRepository _sut;

	public UpdateStatusTests(IssueTrackerTestFactory factory)
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
	public async Task UpdateAsync_With_ValidData_Should_UpdateTheStatus_Test()
	{
		// Arrange
		StatusModel expected = FakeStatus.GetNewStatus();
		await _sut.CreateAsync(expected);

		// Act
		expected.StatusDescription = "Updated";
		await _sut.UpdateAsync(expected.Id, expected);
		StatusModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);
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
}