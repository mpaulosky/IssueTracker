//-----------------------------------------------------------------------// <copyright>//	File:		FakeIssue.cs//	Company:mpaulosky//	Author:	Matthew Paulosky//	Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeIssue class
/// </summary>
public static class FakeIssue{	private static Faker<IssueModel>? _issueGenerator;	private static void SetupGenerator()
	{
		Randomizer.Seed = new Random(123);

		_issueGenerator = new Faker<IssueModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(f => f.Title, f => f.Lorem.Sentence())
			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.ApprovedForRelease, f => f.Random.Bool())
			.RuleFor(f => f.Rejected, f => f.Random.Bool())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(f => f.Category, FakeCategory.GetBasicCategories(1).First())
			.RuleFor(f => f.IssueStatus, FakeStatus.GetBasicStatuses(1).First())
			.RuleFor(f => f.Archived, f => f.Random.Bool());
	}

	/// <summary>
	///   Gets the new issue.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <returns>IssueModel</returns>
	public static IssueModel GetNewIssue(bool keepId = false)
	{
		SetupGenerator();

		var issue = _issueGenerator!.Generate();

		if (!keepId)
		{
			issue.Id = string.Empty;
		}

		issue.Archived = false;

		var status = new StatusModel
		{
			Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString(),
			StatusName = "InWork",
			StatusDescription = "The issue was accepted and it is in work.",
			Archived = false
		};

		issue.IssueStatus = new BasicStatusModel(status);

		return issue;
	}

	/// <summary>
	///   Gets the issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <returns>IEnumerable List of IssueModels</returns>
	public static IEnumerable<IssueModel> GetIssues(int numberOfIssues)
	{
		SetupGenerator();

		var issues = _issueGenerator!.Generate(numberOfIssues);

		foreach (var item in issues.Where(x => x.Archived))
		{
			item.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser());
		}

		return issues;
	}

	/// <summary>
	///   Gets a list of basic issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <returns>IEnumerable List of BasicIssueModels</returns>
	public static IEnumerable<BasicIssueModel> GetBasicIssues(int numberOfIssues)	{		SetupGenerator();		var issues = GetIssues(numberOfIssues);		var basicIssues =			issues.Select(c => new BasicIssueModel(c));		return basicIssues;	}}
