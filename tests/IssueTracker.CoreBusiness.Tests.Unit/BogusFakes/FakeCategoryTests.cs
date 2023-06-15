// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeCategoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCategoryTests
{
	[Theory(DisplayName = "FakeCategory GetNewCategory Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewCategory_With_Boolean_Value_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange

		// Act
		var result = FakeCategory.GetNewCategory(expected);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().BeEquivalentTo(FakeCategory.GetNewCategory(expected),
			options => options.Excluding(t => t.Id));
	}

	[Theory(DisplayName = "FakeCategory GetNewCategory With New Seed Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewCategory_With_UsingNewSeedSetTrue_Should_ReturnRandomValues_Test(bool expected)
	{
		// Arrange

		// Act
		CategoryModel result = FakeCategory.GetNewCategory(expected, true);


		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().NotBeEquivalentTo(FakeCategory.GetNewCategory(expected, true),
			options => options.Excluding(t => t.Id));

	}

	[Fact(DisplayName = "FakeCategory GetCategories Existing Test")]
	public void GetCategories_With_No_Variable_Should_Return_A_List_Of_Categories_Test()
	{
		// Arrange
		const int excludingCount = 5;

		// Act
		var result = FakeCategory.GetCategories();

		// Assert

		result.Count.Should().Be(excludingCount);
		result.Should().BeEquivalentTo(FakeCategory.GetCategories(),
			options => options.Excluding(t => t.Id));
	}

	[Theory(DisplayName = "FakeCategory GetCategories Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetCategories_With_RequestForCategories_Should_ReturnFakeCategories_Test(int countRequested)
	{
		// Arrange

		// Act
		var result = FakeCategory.GetCategories(countRequested);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().BeEquivalentTo(FakeCategory.GetCategories(countRequested),
			options => options.Excluding(t => t.Id));
	}

	[Theory(DisplayName = "FakeCategory GetCategories Test with use new seed")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetCategories_With_UseNewSeed_Should_ReturnFakeCategoriesThatAreDifferent_Test(int countRequested)
	{
		// Arrange

		// Act
		var result = FakeCategory.GetCategories(countRequested, true);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().NotBeEquivalentTo(FakeCategory.GetCategories(countRequested),
			options => options.Excluding(t => t.Id));
	}

	[Fact(DisplayName = "FakeCategory GetBasicCategory Test")]
	public void GetBasicCategories_With_RequestForBasicCategories_Should_ReturnFakeBasicCategories_Test()
	{
		// Arrange
		const int countRequested = 1;

		// Act
		var result = FakeCategory.GetBasicCategories(countRequested);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().BeEquivalentTo(FakeCategory.GetBasicCategories(countRequested));
	}

	[Fact(DisplayName = "FakeCategory GetBasicCategory Test With Use New Seed")]
	public void GetBasicCategories_With_UseNewSeed_Should_ReturnFakeBasicCategories_Test()
	{
		// Arrange
		const int countRequested = 1;

		// Act
		var result = FakeCategory.GetBasicCategories(countRequested, true);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().NotBeEquivalentTo(FakeCategory.GetBasicCategories(countRequested));
	}
}