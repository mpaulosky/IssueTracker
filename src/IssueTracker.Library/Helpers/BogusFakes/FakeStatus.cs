namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeStatus
{

	public static IEnumerable<StatusModel> GetStatuses(int numberOfStatuses)
	{

		var statusGenerator = new Faker<StatusModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		return statusGenerator.Generate(numberOfStatuses);

	}

	public static IEnumerable<BasicStatusModel> GetBasicStatuses(int numberOfStatuses)
	{

		var statusGenerator = new Faker<BasicStatusModel>()
		.RuleFor(x => x.StatusName, f => f.PickRandom<Status>().ToString())
		.RuleFor(x => x.StatusDescription, f => f.Lorem.Sentence());

		var basicComments = statusGenerator.Generate(numberOfStatuses);

		return basicComments;

	}

}

internal enum Status
{

	Answered,
	Watching,
	Dismissed,
	InWork

}