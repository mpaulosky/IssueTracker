//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeIssue.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeIssue class
/// </summary>
public static class FakeIssue
{
	private static Faker<IssueModel>? _issueGenerator;

	private static void SetupGenerator()
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
	/// Gets the new issue.
	/// </summary>
	/// <returns>IssueModel</returns>
	public static IssueModel GetNewIssue()
	{

		SetupGenerator();

		var issue = _issueGenerator!.Generate();

		issue.Id = string.Empty;

		return issue;

	}

	/// <summary>
	/// Gets the issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <returns>IEnumerable List of IssueModels</returns>
	public static IEnumerable<IssueModel> GetIssues(int numberOfIssues)
	{

		SetupGenerator();

		var issues = _issueGenerator!.Generate(numberOfIssues);

		return issues;

	}

	/// <summary>
	/// Gets a list of basic issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <returns>IEnumerable List of BasicIssueModels</returns>
	public static IEnumerable<BasicIssueModel> GetBasicIssues(int numberOfIssues)
	{

		SetupGenerator();

		var issues = _issueGenerator!.Generate(numberOfIssues);

		var basicIssues = issues.Select(c => new BasicIssueModel(c));

		return basicIssues;

	}

}
