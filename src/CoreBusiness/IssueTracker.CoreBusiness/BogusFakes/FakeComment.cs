// Copyright (c) 2023. All rights reserved.
// File Name :     FakeComment.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
///   FakeComment class
/// </summary>
public static class FakeComment
{
	private static Faker<CommentModel>? _commentsGenerator;

	private static void SetupGenerator()
	{
		Randomizer.Seed = new Random(123);

		_commentsGenerator = new Faker<CommentModel>()
			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())
			.RuleFor(c => c.Title, f => f.Lorem.Sentence())
			.RuleFor(c => c.Description, f => f.Lorem.Paragraph())
			.RuleFor(x => x.CommentOnSource, FakeSource.GetSource())
			.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
			.RuleFor(c => c.DateCreated, f => f.Date.Past())
			.RuleFor(f => f.Archived, f => f.Random.Bool());
	}

	/// <summary>
	///   Gets a new comment.
	/// </summary>
	/// <param name="keepId">bool whether to keep the generated Id</param>
	/// <returns>CommentModel</returns>
	public static CommentModel GetNewComment(bool keepId = false)
	{
		SetupGenerator();

		CommentModel? comment = _commentsGenerator!.Generate();

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
	/// <returns>IEnumerable List of CommentModel</returns>
	public static IEnumerable<CommentModel> GetComments(int numberOfComments)
	{
		SetupGenerator();

		List<CommentModel>? comments = _commentsGenerator!.Generate(numberOfComments);

		foreach (CommentModel? comment in comments.Where(comment => comment.Archived))
		{
			comment.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser());
		}

		return comments;
	}

	/// <summary>
	///   Gets a list of basic comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <returns>IEnumerable List of BasicCommentModels</returns>
	public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)
	{
		IEnumerable<CommentModel> comments = GetComments(numberOfComments);

		IEnumerable<BasicCommentModel> basicComments =
			comments.Select(c => new BasicCommentModel(c));

		return basicComments;
	}
}