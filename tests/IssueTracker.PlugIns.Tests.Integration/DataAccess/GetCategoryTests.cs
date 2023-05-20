namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCategoryTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CategoryRepository _sut;
	private const string CleanupValue = "categories";

	public GetCategoryTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CategoryRepository(context);

	}

	[Fact(DisplayName = "GetAsync With Valid Data Should Succeed")]
	public async Task GetAsync_With_WithData_Should_Return_A_Valid_Category_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();
		await _sut.CreateAsync(expected);

		// Act
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}

	[Theory(DisplayName = "GetAsync With In Valid Data Should Fail")]
	[InlineData("62cf2ad6326e99d665759e5a")]
	public async Task GetAsync_With_WithoutData_Should_Return_Nothing_TestAsync(string? value)
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
