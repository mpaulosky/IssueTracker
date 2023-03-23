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

	/// <summary>
	/// Gets the new issue.
	/// </summary>
	/// <returns>IssueModel</returns>
	public static IssueModel GetNewIssue()
	{

		Faker<IssueModel> issueGenerator = new Faker<IssueModel>()
			.RuleFor(f => f.Title, f => f.Lorem.Sentence())
			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.IssueStatus, FakeStatus.GetBasicStatuses(1).First())
			.RuleFor(f => f.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.ApprovedForRelease, f => f.Random.Bool())
			.RuleFor(f => f.Rejected, f => f.Random.Bool())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(f => f.Category, FakeCategory.GetBasicCategories(1).First())
			.RuleFor(f => f.Archived, f => f.Random.Bool());

		IssueModel issue = issueGenerator.Generate();

		return issue;
	}

	/// <summary>
	/// Gets the issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <returns>IEnumerable List of IssueModels</returns>
	public static IEnumerable<IssueModel> GetIssues(int numberOfIssues)
	{

		Faker<IssueModel> issueGenerator = new Faker<IssueModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(f => f.Title, f => f.Lorem.Sentence())
		.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
		.RuleFor(f => f.IssueStatus, FakeStatus.GetBasicStatuses(1).First())
		.RuleFor(f => f.DateCreated, f => f.Date.Past())
		.RuleFor(f => f.ApprovedForRelease, f => f.Random.Bool())
		.RuleFor(f => f.Rejected, f => f.Random.Bool())
		.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(f => f.Category, FakeCategory.GetBasicCategories(1).First())
		.RuleFor(f => f.Archived, f => f.Random.Bool());

		List<IssueModel> issues = issueGenerator.Generate(numberOfIssues);

		return issues;

	}

	/// <summary>
	/// Gets the basic issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <returns>IEnumerable List of BasicIssueModels</returns>
	public static IEnumerable<BasicIssueModel> GetBasicIssues(int numberOfIssues)
	{

		IEnumerable<IssueModel> issues = GetIssues(numberOfIssues);

		IEnumerable<BasicIssueModel> basicIssues = issues.Select(c => new BasicIssueModel(c));

		return basicIssues;

	}

}