
namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCategoryTests
{

	public FakeCategoryTests()
	{
	}

	[Fact(DisplayName = "FakeCategory GetCategories Test")]
	public void GetCategories_With_RequestForCategories_Should_ReturnFakeCategories_Test()
	{

		// Arrange

		// Act
		var result = FakeCategory.GetCategories(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().CategoryName.Should().NotBeNull();
		result.First().CategoryDescription.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeCategory GetBasicCategory Test")]
	public void GetBasicCategories_With_RequestForBasicCategories_Should_ReturnFakeBasicCategories_Test()
	{

		// Arrange

		// Act
		var result = FakeCategory.GetBasicCategories(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().CategoryName.Should().NotBeNull();
		result.First().CategoryDescription.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeCategory GetBasicCategory Test")]
	public void GetBasicCategory_With_RequestForABasicCategory_Should_ReturnAFakeBasicCategory_Test()
	{

		// Arrange

		// Act
		BasicCategoryModel result = FakeCategory.GetBasicCategory();

		// Assert
		result.CategoryName.Should().NotBeNull();
		result.CategoryDescription.Should().NotBeNull();

	}

}