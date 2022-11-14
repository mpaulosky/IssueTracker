//-----------------------------------------------------------------------
// <copyright file="FakeComment.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeComment
{

	public static CommentModel GetNewComment()
	{

		var commentsGenerator = new Faker<CommentModel>()
		.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
		.RuleFor(x => x.Issue, FakeIssue.GetBasicIssues(1).First())
		.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(c => c.DateCreated, f => f.Date.Past());

		var comment = commentsGenerator.Generate();

		return comment;

	}

	public static IEnumerable<CommentModel> GetComments(int numberOfComments)
	{

		var commentsGenerator = new Faker<CommentModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
		.RuleFor(x => x.Issue, FakeIssue.GetBasicIssues(1).First())
		.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(c => c.DateCreated, f => f.Date.Past());

		var comments = commentsGenerator.Generate(numberOfComments);

		return comments;

	}

	public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)
	{

		var commentsGenerator = new Faker<BasicCommentModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(c => c.Comment, f => f.Lorem.Sentence());

		var basicComments = commentsGenerator.Generate(numberOfComments);

		return basicComments;

	}

	public static BasicCommentModel GetBasicComment()
	{

		var commentsGenerator = new Faker<BasicCommentModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(c => c.Comment, f => f.Lorem.Sentence());

		var comment = commentsGenerator.Generate();

		var basicComment = new BasicCommentModel(comment.Id, comment.Comment);

		return basicComment;

	}

}