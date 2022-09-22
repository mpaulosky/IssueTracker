namespace TestingSupport.Library.Fakes;

public static class FakeStatus
{
	public static IEnumerable<StatusModel> GetStatuses(int numberOfStatuses)
	{
		Faker<StatusModel>? statusGenerator = new Faker<StatusModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(x => x.StatusName, f => f.PickRandom<StatusEnum>().ToString())
			.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());
		
		return statusGenerator.Generate(numberOfStatuses);
	}
}

public enum StatusEnum
{
	Answered,
	Watching,
	Dismissed,
	InWork
}