﻿namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class BasicCategoryModelTests
{

	[Fact]
	public void BasicCategoryModel_With_CategoryModel_Test()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

		// Act
		var result = new BasicCategoryModel(expected);

		// Assert
		result.CategoryDescription.Should().Be(expected.CategoryDescription);
		result.CategoryName.Should().Be(expected.CategoryName);

	}

	[Fact()]
	public void BasicCategoryModel_With_Values_Test()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory();

		// Act
		var result = new BasicCategoryModel(expected.CategoryName, expected.CategoryDescription);

		// Assert
		result.CategoryDescription.Should().Be(expected.CategoryDescription);
		result.CategoryName.Should().Be(expected.CategoryName);

	}

}
