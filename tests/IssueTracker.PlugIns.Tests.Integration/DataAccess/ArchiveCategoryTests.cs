﻿
namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class ArchiveCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private string? _cleanupValue;

	public ArchiveCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "Archive Category With Valid Data (Archive)")]
	public async Task ArchiveAsync_With_ValidData_Should_ArchiveACategory_TestAsync()
	{

		// Arrange
		_cleanupValue = "categories";
		var expected = FakeCategory.GetNewCategory();

		await _sut.CreateAsync(expected);

		// Act
		await _sut.ArchiveAsync(expected);

		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Archived.Should().BeTrue();

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
