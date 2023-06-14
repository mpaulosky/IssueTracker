// Copyright (c) 2023. All rights reserved.
// File Name :     FakeCategory.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeCategory class
/// </summary>
public static class FakeCategory
{
	private static Faker<CategoryModel>? _categoryGenerator;

	private static void SetupGenerator()
	{
		Randomizer.Seed = new Random(123);

		_categoryGenerator = new Faker<CategoryModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
			.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence())
			.RuleFor(f => f.Archived, f => f.Random.Bool());
	}

	/// <summary>
	///   Gets a new category
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <returns>CategoryModel</returns>
	public static CategoryModel GetNewCategory(bool keepId = false)
	{
		SetupGenerator();

		CategoryModel? category = _categoryGenerator!.Generate();

		if (!keepId)
		{
			category.Id = string.Empty;
		}

		category.Archived = false;

		return category;
	}

	/// <summary>
	///   Gets a list of categories that exit.
	/// </summary>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<CategoryModel> GetCategories()
	{
		List<CategoryModel> categories = new()
		{
			new CategoryModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				CategoryDescription = "An Issue with the design.",
				CategoryName = "Design",
				Archived = false
			},
			new CategoryModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				CategoryDescription = "An Issue with the documentation.",
				CategoryName = "Documentation",
				Archived = false
			},
			new CategoryModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				CategoryDescription = "An Issue with the implementation.",
				CategoryName = "Implementation",
				Archived = false
			},
			new CategoryModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				CategoryDescription = "An Issue needs clarification.",
				CategoryName = "Clarification",
				Archived = false
			},
			new CategoryModel
			{
				Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
				CategoryDescription = "A miscellaneous Issue.",
				CategoryName = "Miscellaneous",
				Archived = false
			}
		};

		return categories;
	}

	/// <summary>
	///   Gets a list of categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{
		SetupGenerator();

		List<CategoryModel>? categories = _categoryGenerator!.Generate(numberOfCategories);

		return categories;
	}

	/// <summary>
	///   Gets the basic categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <returns>IEnumerable List of CategoryModels</returns>
	public static IEnumerable<BasicCategoryModel> GetBasicCategories(int numberOfCategories)
	{
		SetupGenerator();

		List<CategoryModel>? categories = _categoryGenerator!.Generate(numberOfCategories);

		return (from category in categories
						let basicCategory = new BasicCategoryModel(category)
						select basicCategory).ToList();
	}

	/// <summary>
	///   Gets the basic category.
	/// </summary>
	/// <returns>BasicCategoryModel</returns>
	public static BasicCategoryModel GetBasicCategory()
	{
		SetupGenerator();

		CategoryModel? category = _categoryGenerator!.Generate();

		return new BasicCategoryModel(category);
	}
}