using IssueTracker.Library.Contracts;
using IssueTracker.Library.DataAccess;
using IssueTracker.Library.Helpers.BogusFakes;
using IssueTracker.Library.Services;

using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.UI.Tests.Integration.Services.CategoryServicesTests;

[ExcludeFromCodeCoverage]
public class GetCategoryTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly CategoryService _sut;

	public GetCategoryTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = _factory.Services.GetRequiredService(typeof(ICategoryRepository));
		var memCache = _factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CategoryService((ICategoryRepository)repo, (IMemoryCache)memCache);

	}

	[Fact()]
	public async Task GetCategory_With_StateUnderTest_Should_ExpectedBehaviour_TestAsync()
	{

		// Arrange
		var expected = FakeCategory.GetCategories(1).ToList()[0];
		await _sut.CreateCategory(expected);

		// Act
		var result = await _sut.GetCategory(expected.Id);

		// Assert
		result.Should().BeEquivalentTo(expected);

	}
}