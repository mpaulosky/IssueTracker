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

	/// <summary>
	/// Gets the new comment.
	/// </summary>
	/// <returns>CommentModel</returns>
	public static CommentModel GetNewComment()
	{

		Faker<CommentModel> commentsGenerator = new Faker<CommentModel>()
		.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
		.RuleFor(x => x.Source, FakeSource.GetNewSource())
		.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(c => c.DateCreated, f => f.Date.Past());

		CommentModel comment = commentsGenerator.Generate();

		return comment;

	}

	/// <summary>
	/// Gets the comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <returns>IEnumerable List of CommentModel</returns>
	public static IEnumerable<CommentModel> GetComments(int numberOfComments)
	{

		Faker<CommentModel> commentsGenerator = new Faker<CommentModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
		.RuleFor(x => x.Source, FakeSource.GetNewSource())
		.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(c => c.DateCreated, f => f.Date.Past());

		List<CommentModel> comments = commentsGenerator.Generate(numberOfComments);

		return comments;

	}

	/// <summary>
	/// Gets the basic comments.
	/// </summary>
	/// <param name="numberOfComments">The number of comments.</param>
	/// <returns>IEnumerable List of BasicCommentModels</returns>
	public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)
	{

		Faker<BasicCommentModel> commentsGenerator = new Faker<BasicCommentModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(c => c.Comment, f => f.Lorem.Sentence());

		List<BasicCommentModel> basicComments = commentsGenerator.Generate(numberOfComments);

		return basicComments;

	}

	/// <summary>
	/// Gets the basic comment.
	/// </summary>
	/// <returns>BasicCommentModel</returns>
	public static BasicCommentModel GetBasicComment()
	{

		Faker<BasicCommentModel> commentsGenerator = new Faker<BasicCommentModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(c => c.Comment, f => f.Lorem.Sentence());

		BasicCommentModel comment = commentsGenerator.Generate();

		var basicComment = new BasicCommentModel(comment.Id, comment.Comment);

		return basicComment;

	}

}