namespace TestingSupport.Library.Fakes;

public static class FakeIssue
{
	public static IEnumerable<IssueModel> GetIssues(int numberOfIssues)
	{
		Faker<IssueModel>? issueGenerator = new Faker<IssueModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(f => f.IssueName, f => f.Lorem.Sentence())
			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.IssueStatus, new BasicStatusModel(FakeStatus.GetStatuses(1).First()))
			.RuleFor(f => f.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.ApprovedForRelease, f => f.Random.Bool())
			.RuleFor(f => f.Rejected, f => f.Random.Bool())
			.RuleFor(f => f.Author, new BasicUserModel(FakeUser.GetUsers(1).First()))
			.RuleFor(f => f.Category, new BasicCategoryModel(FakeCategory.GetCategories(1).First()))
			.RuleFor(f => f.Archived, f => f.Random.Bool());

		return issueGenerator.Generate(numberOfIssues);
	}
	
	public static IEnumerable<BasicIssueModel> GetBasicIssues(int numberOfIssues)
	{
		IEnumerable<IssueModel> issues = GetIssues(numberOfIssues);
		IEnumerable<BasicIssueModel> basicIssues = issues.Select(c => new BasicIssueModel(c));
		return basicIssues;
	}
}