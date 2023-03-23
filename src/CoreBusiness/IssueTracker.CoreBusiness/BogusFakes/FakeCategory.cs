//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeCategory.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------


namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeCategory class
/// </summary>
public static class FakeCategory
{

	/// <summary>
	/// Gets the new category.
	/// </summary>
	/// <returns>CategoryModel</returns>
	public static CategoryModel GetNewCategory()
	{

		Faker<CategoryModel> categoryGenerator = new Faker<CategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		CategoryModel category = categoryGenerator.Generate();

		return category;

	}

	/// <summary>
	/// Gets the categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{

		Faker<CategoryModel> categoryGenerator = new Faker<CategoryModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		List<CategoryModel> categories = categoryGenerator.Generate(numberOfCategories);

		return categories;

	}

	/// <summary>
	/// Gets the basic categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<BasicCategoryModel> GetBasicCategories(int numberOfCategories)
	{

		Faker<BasicCategoryModel> categoryGenerator = new Faker<BasicCategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		List<BasicCategoryModel> basicCategories = categoryGenerator.Generate(numberOfCategories);

		return basicCategories;

	}

	/// <summary>
	/// Gets the basic category.
	/// </summary>
	/// <returns>BasicCategoryModel</returns>
	public static BasicCategoryModel GetBasicCategory()
	{
		Faker<BasicCategoryModel> categoryGenerator = new Faker<BasicCategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		BasicCategoryModel category = categoryGenerator.Generate();

		return category;

	}

}

/// <summary>
/// Category enum
/// </summary>
internal enum Category
{

	Design,
	Documentation,
	Implementation,
	Clarification,
	Miscellaneous

}