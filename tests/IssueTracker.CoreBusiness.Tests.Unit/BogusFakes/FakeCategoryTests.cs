
namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCategoryTests
{

	public FakeCategoryTests()
	{
	}


	[Theory(DisplayName = "FakeCategory GetNewCategory Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewCategory_With_Boolean_Value_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{

		// Arrange

		// Act
		var result = FakeCategory.GetNewCategory(expected);

		// Assert
		switch (expected)
		{
			case true:
				result.Id.Should().NotBeNull();
				break;
			default:
				result.Id.Should().BeEmpty();
				break;
		}
		result.Id.Should().NotBeNull();
		result.CategoryName.Should().NotBeNull();
		result.CategoryDescription.Should().NotBeNull();
		result.Archived.Should().BeFalse();

	}

	[Fact(DisplayName = "FakeCategory GetCategories Existing Test")]
	public void GetCategories_With_No_Varriable_Should_Return_A_List_Of_Categories_Test()
	{

		// Arrange
		const int expected = 5;

		// Act
		var result = FakeCategory.GetCategories().ToList();

		// Assert
		result.Count.Should().Be(expected);
		result.First().Id.Should().NotBeNull();
		result.First().CategoryName.Should().Be("Design");
		result.First().CategoryDescription.Should().Be("An Issue with the design.");
		result.First().Archived.Should().BeFalse();

	}

	[Theory(DisplayName = "FakeCategory GetCategories Test")]
	[InlineData(1)]
	[InlineData(2)]
	public void GetCategories_With_RequestForCategories_Should_ReturnFakeCategories_Test(int countRequested)
	{

		// Arrange

		// Act
		var result = FakeCategory.GetCategories(countRequested).ToList();

		// Assert
		result.Count.Should().Be(countRequested);
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
