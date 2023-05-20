﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class CreateCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private const string CleanupValue = "categories";

	public CreateCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "CreateAsync Category With Valid Data Should Succeed")]
	public async Task CreateAsync_With_ValidData_Should_CreateACategory_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

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

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
