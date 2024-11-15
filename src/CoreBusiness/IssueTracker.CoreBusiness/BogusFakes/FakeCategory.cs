// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeCategory.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeCategory class
/// </summary>
public static class FakeCategory
{
	/// <summary>
	///   Gets a new category
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>CategoryModel</returns>
	public static CategoryModel GetNewCategory(bool keepId = false, bool useNewSeed = false)
	{
		CategoryModel? category = GenerateFake(useNewSeed).Generate();

		if (!keepId)
		{
			category.Id = string.Empty;
		}

		category.Archived = false;

		return category;
	}

	/// <summary>
	///   Gets a list of categories that exist.
	/// </summary>
	/// <returns>A Default List of CategoryModels</returns>
	public static List<CategoryModel> GetCategories()
	{
		List<CategoryModel> categories =
		[
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
		];

		return categories;
	}

	/// <summary>
	///   Gets a list of categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of CategoryModels</returns>
	public static List<CategoryModel> GetCategories(int numberOfCategories, bool useNewSeed = false)
	{
		List<CategoryModel>? categories = GenerateFake(useNewSeed).Generate(numberOfCategories);

		foreach (CategoryModel? category in categories.Where(x => x.Archived))
		{
			category.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser(true));
		}

		return categories;
	}

	/// <summary>
	///   Gets the basic categories.
	/// </summary>
	/// <param name="numberOfCategories">The number of categories.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of BasicCategoryModel</returns>
	public static List<BasicCategoryModel> GetBasicCategories(int numberOfCategories, bool useNewSeed = false)
	{
		List<CategoryModel>? categories = GenerateFake(useNewSeed).Generate(numberOfCategories);

		return categories.Select(c => new BasicCategoryModel(c)).ToList();
	}

	/// <summary>
	///   GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A Faker CategoryModel</returns>
	private static Faker<CategoryModel> GenerateFake(bool useNewSeed = false)
	{
		int seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<CategoryModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
			.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence())
			.RuleFor(f => f.Archived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}