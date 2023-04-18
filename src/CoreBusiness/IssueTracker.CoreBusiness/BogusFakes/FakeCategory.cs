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

	private static Faker<CategoryModel>? _categoryGenerator;

	private static void SetupGenerator()
	{

		Randomizer.Seed = new Random(123);

		_categoryGenerator = new Faker<CategoryModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

	}

	/// <summary>
	/// Gets a new category
	/// </summary>
	/// <returns>CategoryModel</returns>
	public static CategoryModel GetNewCategory()
	{

		SetupGenerator();

		var category = _categoryGenerator!.Generate();

		category.Id = string.Empty;

		return category;

	}

	/// <summary>
	/// Gets a list of categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{

		SetupGenerator();

		var categories = _categoryGenerator!.Generate(numberOfCategories);

		return categories;

	}

	/// <summary>
	/// Gets the basic categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<BasicCategoryModel> GetBasicCategories(int numberOfCategories)
	{

		SetupGenerator();

		var categories = _categoryGenerator!.Generate(numberOfCategories);

		return (from category in categories
						let basicCategory = new BasicCategoryModel(category)
						select basicCategory).ToList();

	}

	/// <summary>
	/// Gets the basic category.
	/// </summary>
	/// <returns>BasicCategoryModel</returns>
	public static BasicCategoryModel GetBasicCategory()
	{

		SetupGenerator();

		var category = _categoryGenerator!.Generate();

		return new BasicCategoryModel(category);

	}

}
