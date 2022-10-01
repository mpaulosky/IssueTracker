namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeCategory
{

	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{

		var categoryGenerator = new Faker<CategoryModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		var categories = categoryGenerator.Generate(numberOfCategories);

		return categories;

	}

	public static IEnumerable<BasicCategoryModel> GetBasicCategories(int numberOfCategories)
	{
		var categoryGenerator = new Faker<BasicCategoryModel>()
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		var basicCategories = categoryGenerator.Generate(numberOfCategories);

		return basicCategories;

	}

	public static BasicCategoryModel GetBasicCategory()
	{
		var categoryGenerator = new Faker<CategoryModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString())
		.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
		.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		var category = categoryGenerator.Generate();

		var basicCategory = new BasicCategoryModel(category);

		return basicCategory;

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