// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeIssue.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeIssue class
/// </summary>
public static class FakeIssue
{
	/// <summary>
	///   Gets the new issue.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>IssueModel</returns>
	public static IssueModel GetNewIssue(bool keepId = false, bool useNewSeed = false)
	{
		var issue = GenerateFake(useNewSeed).Generate();

		if (!keepId)
		{
			issue.Id = string.Empty;
		}

		issue.Archived = false;

		var statuses = FakeStatus.GetStatuses();

		issue.IssueStatus = new BasicStatusModel(statuses[2]);

		return issue;
	}

	/// <summary>
	///   Gets the issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of IssueModels</returns>
	public static List<IssueModel> GetIssues(int numberOfIssues, bool useNewSeed = false)
	{
		var issues = GenerateFake(useNewSeed).Generate(numberOfIssues);

		foreach (var issue in issues.Where(x => x.Archived))
		{
			issue.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser(true));
		}

		return issues;
	}

	/// <summary>
	///   Gets a list of basic issues.
	/// </summary>
	/// <param name="numberOfIssues">The number of issues.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of BasicIssueModels</returns>
	public static List<BasicIssueModel> GetBasicIssues(int numberOfIssues, bool useNewSeed = false)
	{
		List<IssueModel> issues = GenerateFake(useNewSeed).Generate(numberOfIssues);

		return issues.Select(c => new BasicIssueModel(c)).ToList();
	}

	/// <summary>
	/// GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A Faker IssueModel</returns>
	private static Faker<IssueModel> GenerateFake(bool useNewSeed = false)
	{
		var seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<IssueModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(f => f.Title, f => f.Lorem.Sentence())
			.RuleFor(f => f.Description, f => f.Lorem.Paragraph())
			.RuleFor(f => f.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.ApprovedForRelease, f => f.Random.Bool())
			.RuleFor(f => f.Rejected, f => f.Random.Bool())
			.RuleFor(f => f.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(f => f.Category, FakeCategory.GetBasicCategories(1).First())
			.RuleFor(f => f.IssueStatus, FakeStatus.GetBasicStatuses(1).First())
			.RuleFor(f => f.Archived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}