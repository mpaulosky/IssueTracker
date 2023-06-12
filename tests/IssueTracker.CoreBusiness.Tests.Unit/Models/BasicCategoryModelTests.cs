﻿// Copyright (c) 2023. All rights reserved.
// File Name :     BasicCategoryModelTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit

namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class BasicCategoryModelTests
{
	[Fact]
	public void BasicCategoryModel_With_CategoryModel_Test()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory();

		// Act
		BasicCategoryModel result = new BasicCategoryModel(expected);

		// Assert
		result.CategoryDescription.Should().Be(expected.CategoryDescription);
		result.CategoryName.Should().Be(expected.CategoryName);
	}

	[Fact]
	public void BasicCategoryModel_With_Values_Test()
	{
		// Arrange
		CategoryModel expected = FakeCategory.GetNewCategory();

		// Act
		BasicCategoryModel result = new BasicCategoryModel(expected.CategoryName, expected.CategoryDescription);

		// Assert
		result.CategoryDescription.Should().Be(expected.CategoryDescription);
		result.CategoryName.Should().Be(expected.CategoryName);
	}
}