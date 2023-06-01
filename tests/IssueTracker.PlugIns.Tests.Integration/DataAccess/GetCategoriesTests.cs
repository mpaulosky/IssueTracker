﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCategoriesTests : IAsyncLifetime
{
	private const string CleanupValue = "categories";

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;

	public GetCategoriesTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
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

	[Fact(DisplayName = "GetAllAsync Categories With Valid Data Should Succeed")]
	public async Task GetAllAsync_With_ValidData_Should_ReturnCategories_Test()
	{
		// Arrange
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateAsync(expected);

		// Act
		var results = (await _sut.GetAllAsync()).ToList();

		// Assert
		results.Count.Should().Be(1);
		results.Last().CategoryName.Should().Be(expected.CategoryName);
		results.Last().CategoryDescription.Should().Be(expected.CategoryDescription);
	}
}
