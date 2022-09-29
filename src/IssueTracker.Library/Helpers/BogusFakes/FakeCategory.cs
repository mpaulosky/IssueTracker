namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeCategory
{

	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{

		var categoryGenerator = new Faker<CategoryModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(x => x.CategoryName, f => f.PickRandom<Category>().ToString())
			.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());

		return categoryGenerator.Generate(numberOfCategories);

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