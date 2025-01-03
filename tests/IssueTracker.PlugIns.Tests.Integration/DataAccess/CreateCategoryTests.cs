﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CreateCategoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateCategoryTests : IAsyncLifetime
{
	private const string CleanupValue = "categories";

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;

	public CreateCategoryTests(IssueTrackerTestFactory factory)
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

	[Fact(DisplayName = "CreateAsync Category With Valid Data Should Succeed")]
	public async Task CreateAsync_With_ValidData_Should_CreateACategory_TestAsync()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory();

		// Act
		await _sut.CreateAsync(expected);

		// Assert
		expected.Id.Should().NotBeNull();
	}

	[Fact(DisplayName = "CreateAsync Category With In Valid Data Should Fail")]
	public async Task CreateAsync_With_InValidData_Should_FailToCreateACategory_TestAsync()
	{
		// Arrange

		// Act

		// Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.CreateAsync(null!));
	}
}