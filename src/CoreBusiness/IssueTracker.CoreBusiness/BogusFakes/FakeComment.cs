// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeComment.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeComment class
/// </summary>
public static class FakeComment
{
	/// <summary>
	///   Gets a new comment.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>CommentModel</returns>
	public static CommentModel GetNewComment(bool keepId = false, bool useNewSeed = false)
	{
		CommentModel? comment = GenerateFake(useNewSeed).Generate();

		if (!keepId)
		{
			comment.Id = string.Empty;
		}

		comment.Archived = false;

		return comment;
	}

	/// <summary>
	///   Gets a list of comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of CommentModel</returns>
	public static List<CommentModel> GetComments(int numberOfComments, bool useNewSeed = false)
	{
		List<CommentModel>? comments = GenerateFake(useNewSeed).Generate(numberOfComments);

		foreach (CommentModel? comment in comments.Where(comment => comment.Archived))
		{
			comment.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser(true));
		}

		return comments;
	}

	/// <summary>
	///   Gets a list of basic comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>A List of BasicCommentModels</returns>
	public static List<BasicCommentModel> GetBasicComments(int numberOfComments, bool useNewSeed = false)
	{
		List<CommentModel>? comments = GenerateFake(useNewSeed).Generate(numberOfComments);

		return comments.Select(c => new BasicCommentModel(c)).ToList();
	}

	/// <summary>
	///   GenerateFake method
	/// </summary>
	/// <param name="useNewSeed">bool whether to use a seed other than 0</param>
	/// <returns>Fake CommentModel</returns>
	private static Faker<CommentModel> GenerateFake(bool useNewSeed = false)
	{
		int seed = 0;
		if (useNewSeed)
		{
			seed = Random.Shared.Next(10, int.MaxValue);
		}

		return new Faker<CommentModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(c => c.Title, f => f.Lorem.Sentence())
			.RuleFor(c => c.Description, f => f.Lorem.Paragraph())
			.RuleFor(x => x.Issue, FakeIssue.GetBasicIssues(1).First())
			.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(c => c.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.Archived, f => f.Random.Bool())
			.UseSeed(seed);
	}
}