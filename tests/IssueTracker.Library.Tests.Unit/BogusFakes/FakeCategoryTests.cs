namespace IssueTracker.Library.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCategoryTests
{

	[Fact]
	public void GetCategories_With_RequestForCategories_Should_ReturnFakeCategories_Test()
	{

		// Arrange

		// Act
		var result = FakeCategory.GetCategories(1);

		// Assert
		result.Count().Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().CategoryName.Should().NotBeNull();
		result.First().CategoryDescription.Should().NotBeNull();

	}

	[Fact]
	public void GetBasicCategories_With_RequestForBasicCategories_Should_ReturnFakeBasicCategories_Test()
	{

		// Arrange

		// Act
		var result = FakeCategory.GetBasicCategories(1);

		// Assert
		result.Count().Should().Be(1);
		result.First().CategoryName.Should().NotBeNull();
		result.First().CategoryDescription.Should().NotBeNull();

	}

	[Fact]
	public void GetBasicCategory_With_RequestForABasicCategory_Should_ReturnAFakeBasicCategory_Test()
	{

		// Arrange

		// Act
		var result = FakeCategory.GetBasicCategory();

		// Assert
		result.CategoryName.Should().NotBeNull();
		result.CategoryDescription.Should().NotBeNull();

	}

}