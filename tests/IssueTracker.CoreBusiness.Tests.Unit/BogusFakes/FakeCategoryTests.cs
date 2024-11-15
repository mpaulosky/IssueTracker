// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeCategoryTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

using Bogus;

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
		CategoryModel result = FakeCategory.GetNewCategory(expected);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }

		result.Should().BeEquivalentTo(FakeCategory.GetNewCategory(expected),
			options => options.Excluding(t => t.Id));
	}

	[Theory(DisplayName = "FakeCategory GetNewCategory With New Seed Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewCategory_Using_NewSeed_SetTrue_Should_ReturnRandomValues_Test(bool expected)
	{
		// Arrange

		// Act
		CategoryModel result = FakeCategory.GetNewCategory(expected, true);


		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }

		result.Should().NotBeEquivalentTo(FakeCategory.GetNewCategory(expected, true));
	}

	[Fact(DisplayName = "FakeCategory GetCategories Existing Test")]
	public void GetCategories_With_No_Variable_Should_Return_A_List_Of_Categories_Test()
	{
		// Arrange
		const int excludingCount = 5;

		// Act
		List<CategoryModel> result = FakeCategory.GetCategories();

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
		List<CategoryModel> result = FakeCategory.GetCategories(countRequested);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().BeEquivalentTo(FakeCategory.GetCategories(countRequested),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.ArchivedBy.Id));
	}

	[Theory(DisplayName = "FakeCategory GetCategories Test with use new seed")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetCategories_With_UseNewSeed_Should_ReturnFakeCategoriesThatAreDifferent_Test(int countRequested)
	{
		// Arrange

		// Act
		List<CategoryModel> result = FakeCategory.GetCategories(countRequested, true);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().NotBeEquivalentTo(FakeCategory.GetCategories(countRequested, true));
	}

	[Fact(DisplayName = "FakeCategory GetBasicCategory Test")]
	public void GetBasicCategories_With_RequestForBasicCategories_Should_ReturnFakeBasicCategories_Test()
	{
		// Arrange
		const int countRequested = 1;

		// Act
		List<BasicCategoryModel> result = FakeCategory.GetBasicCategories(countRequested);

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
		List<BasicCategoryModel> result = FakeCategory.GetBasicCategories(countRequested, true);

		// Assert
		result.Count.Should().Be(countRequested);
		result.Should().NotBeEquivalentTo(FakeCategory.GetBasicCategories(countRequested));
	}
	
	
	[Fact]
	public void GenerateFake_Should_ReturnFakerInstance_Test()
	{
		// Act

		var faker = typeof(FakeCategory).GetMethod("GenerateFake",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
			?.Invoke(null, [false]) as Faker<CategoryModel>;

		// Assert

		faker.Should().NotBeNull();
		faker.Should().BeOfType<Faker<CategoryModel>>();
	}
}