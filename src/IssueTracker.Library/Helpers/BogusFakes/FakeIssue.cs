namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeIssue
{

	public static IEnumerable<IssueModel> GetIssues(int numberOfIssues)
	{

		var issueGenerator = new Faker<IssueModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(f => f.IssueName, f => f.Lorem.Sentence())
		.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
		.RuleFor(f => f.IssueStatus, FakeStatus.GetBasicStatuses(1).First())
		.RuleFor(f => f.DateCreated, f => f.Date.Past())
		.RuleFor(f => f.ApprovedForRelease, f => f.Random.Bool())
		.RuleFor(f => f.Rejected, f => f.Random.Bool())
		.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(f => f.Category, FakeCategory.GetBasicCategories(1).First())
		.RuleFor(f => f.Archived, f => f.Random.Bool());

		var issues = issueGenerator.Generate(numberOfIssues);

		return issues;

	}

	public static IEnumerable<BasicIssueModel> GetBasicIssues(int numberOfIssues)
	{

		var issues = GetIssues(numberOfIssues);

		var basicIssues = issues.Select(c => new BasicIssueModel(c));

		return basicIssues;

	}

}