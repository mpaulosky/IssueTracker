// Copyright (c) 2023. All rights reserved.
// File Name :     GetCategoriesTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

namespace IssueTracker.PlugIns.DataAccess;

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

	[Fact(DisplayName = "GetAllAsync Categories With Valid Data Should Succeed")]
	public async Task GetAllAsync_With_ValidData_Should_ReturnCategories_Test()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory();
		await _sut.CreateAsync(expected);

		// Act
		List<CategoryModel> results = (await _sut.GetAllAsync()).ToList();

		// Assert
		results.Count.Should().Be(1);
		results.Last().CategoryName.Should().Be(expected.CategoryName);
		results.Last().CategoryDescription.Should().Be(expected.CategoryDescription);
	}
}