//-----------------------------------------------------------------------
// <copyright>
//	File:		FakeComment.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.BogusFakes;













/// <summary>///   FakeComment class/// </summary>                                                                                                          public static class FakeComment{	private static Faker<CommentModel>? _commentsGenerator;	private static void SetupGenerator()	{		Randomizer.Seed = new Random(123);		_commentsGenerator = new Faker<CommentModel>()			.RuleFor(x => x.Id, new BsonObjectId(ObjectId.GenerateNewId()).ToString())			.RuleFor(c => c.Title, f => f.Lorem.Sentence())			.RuleFor(c => c.Description, f => f.Lorem.Paragraph())			.RuleFor(x => x.CommentOnSource, FakeSource.GetSource())			.RuleFor(c => c.Author, FakeUser.GetBasicUser(1).First())			.RuleFor(c => c.DateCreated, f => f.Date.Past())			.RuleFor(f => f.Archived, f => f.Random.Bool());	}











	/// <summary>	///   Gets a new comment.	/// </summary>	/// <param name="keepId">bool whether to keep the generated Id</param>	/// <returns>CommentModel</returns>                                                                                                                                                                            public static CommentModel GetNewComment(bool keepId = false)	{		SetupGenerator();		var comment = _commentsGenerator!.Generate();		if (!keepId)		{			comment.Id = string.Empty;		}		comment.Archived = false;		return comment;	}











	/// <summary>	///   Gets a list of comments.	/// </summary>	/// <param name="numberOfComments">The number of comments.</param>	/// <returns>IEnumerable List of CommentModel</returns>                                                                                                                                                                                                public static IEnumerable<CommentModel> GetComments(int numberOfComments)	{		SetupGenerator();		var comments = _commentsGenerator!.Generate(numberOfComments);		foreach (var comment in comments.Where(comment => comment.Archived))		{			comment.ArchivedBy = new BasicUserModel(FakeUser.GetNewUser());		}		return comments;	}











	/// <summary>	///   Gets a list of basic comments.	/// </summary>	/// <param name="numberOfComments">The number of comments.</param>	/// <returns>IEnumerable List of BasicCommentModels</returns>                                                                                                                                                                                                            public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)	{		var comments = GetComments(numberOfComments);		var basicComments =			comments.Select(c => new BasicCommentModel(c));		return basicComments;	}}