//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeComment.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;

/// <summary>
/// FakeComment class
/// </summary>
public static class FakeComment
{
	private static Faker<CommentModel>? _commentsGenerator;

	private static void SetupGenerator()
	{

		Randomizer.Seed = new Random(123);

		_commentsGenerator = new Faker<CommentModel>()
				.RuleFor(x => x.Id, Guid.NewGuid().ToString)
				.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
				.RuleFor(x => x.Source, FakeSource.GetSource())
				.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
				.RuleFor(c => c.DateCreated, f => f.Date.Past());

	}

	/// <summary>
	/// Gets a new comment.
	/// </summary>
	/// <returns>CommentModel</returns>
	public static CommentModel GetNewComment()
	{

		SetupGenerator();

		CommentModel comment = _commentsGenerator!.Generate();

		comment.Id = string.Empty;

		return comment;

	}

	/// <summary>
	/// Gets a list of comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <returns>IEnumerable List of CommentModel</returns>
	public static IEnumerable<CommentModel> GetComments(int numberOfComments)
	{

		SetupGenerator();

		var comments = _commentsGenerator!.Generate(numberOfComments);

		return comments;

	}

	/// <summary>
	/// Gets a list of basic comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <returns>IEnumerable List of BasicCommentModels</returns>
	public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)
	{

		SetupGenerator();

		var comments = _commentsGenerator!.Generate(numberOfComments);

		var basicComments = comments.Select(comments => new BasicCommentModel(comments));

		return basicComments;

	}

	/// <summary>
	/// Gets the basic comment.
	/// </summary>
	/// <returns>BasicCommentModel</returns>
	public static BasicCommentModel GetBasicComment()
	{

		SetupGenerator();

		var comment = _commentsGenerator!.Generate();

		var basicComment = new BasicCommentModel(comment.Id, comment.Comment);

		return basicComment;

	}

}