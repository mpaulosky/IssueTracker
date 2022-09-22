namespace TestingSupport.Library.Fakes;

public static class FakeCategory
{
	public static IEnumerable<CategoryModel> GetCategories(int numberOfCategories)
	{
		Faker<CategoryModel>? categoryGenerator = new Faker<CategoryModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(x => x.CategoryName, f => f.PickRandom<CategoryEnum>().ToString())
			.RuleFor(x => x.CategoryDescription, f => f.Lorem.Sentence());
		
		return categoryGenerator.Generate(numberOfCategories);
	}
}

public enum CategoryEnum
{
	Design,
	Documentation,
	Implementation,
	Clarification,
	Miscellaneous
}