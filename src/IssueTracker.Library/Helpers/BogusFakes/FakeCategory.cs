//-----------------------------------------------------------------------
// <copyright file="FakeCategory.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeCategory
{

	public static CategoryModel GetNewCategory()
	{

		Faker<CategoryModel> categoryGenerator = new Faker<CategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		CategoryModel category = categoryGenerator.Generate();

		return category;

	}

	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{

		Faker<CategoryModel> categoryGenerator = new Faker<CategoryModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		List<CategoryModel> categories = categoryGenerator.Generate(numberOfCategories);

		return categories;

	}

	public static IEnumerable<BasicCategoryModel> GetBasicCategories(int numberOfCategories)
	{

		Faker<BasicCategoryModel> categoryGenerator = new Faker<BasicCategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		List<BasicCategoryModel> basicCategories = categoryGenerator.Generate(numberOfCategories);

		return basicCategories;

	}

	public static BasicCategoryModel GetBasicCategory()
	{
		Faker<BasicCategoryModel> categoryGenerator = new Faker<BasicCategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		BasicCategoryModel category = categoryGenerator.Generate();

		return category;

	}

}

internal enum Category
{

	Design,
	Documentation,
	Implementation,
	Clarification,
	Miscellaneous

}