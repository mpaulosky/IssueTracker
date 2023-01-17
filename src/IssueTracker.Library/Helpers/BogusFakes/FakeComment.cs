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

		Faker<CommentModel> commentsGenerator = new Faker<CommentModel>()
		.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
		.RuleFor(x => x.Issue, FakeIssue.GetBasicIssues(1).First())
		.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(c => c.DateCreated, f => f.Date.Past());

		CommentModel comment = commentsGenerator.Generate();

		return comment;

	}

	public static IEnumerable<CommentModel> GetComments(int numberOfComments)
	{

		Faker<CommentModel> commentsGenerator = new Faker<CommentModel>()
		.RuleFor(x => x.Id, Guid.NewGuid().ToString)
		.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
		.RuleFor(x => x.Issue, FakeIssue.GetBasicIssues(1).First())
		.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())
		.RuleFor(c => c.DateCreated, f => f.Date.Past());

		List<CommentModel> comments = commentsGenerator.Generate(numberOfComments);

		return comments;

	}

	public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)
	{

		Faker<BasicCommentModel> commentsGenerator = new Faker<BasicCommentModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(c => c.Comment, f => f.Lorem.Sentence());

		List<BasicCommentModel> basicComments = commentsGenerator.Generate(numberOfComments);

		return basicComments;

	}

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