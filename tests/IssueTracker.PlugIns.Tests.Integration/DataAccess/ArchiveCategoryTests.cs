// Copyright (c) 2023. All rights reserved.
// File Name :     ArchiveCategoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveCategoryTests : IAsyncLifetime
{
	private const string CleanupValue = "categories";

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;

	public ArchiveCategoryTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		IMongoDbContextFactory context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact(DisplayName = "Archive Category With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveACategory_TestAsync()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		CategoryModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();
	}
}
