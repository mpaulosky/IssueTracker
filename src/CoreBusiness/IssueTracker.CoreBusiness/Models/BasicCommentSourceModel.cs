//-----------------------------------------------------------------------
// <copyright>
//	File:		BasicCommentSourceModel.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
/// BasicCommentSourceModel
/// </summary>
[Serializable]
public class BasicCommentSourceModel
{

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentSourceModel"/> class.
	/// </summary>
	public BasicCommentSourceModel()
	{

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentSourceModel"/> class.
	/// </summary>
	/// <param name="issue">The issue.</param>
	public BasicCommentSourceModel(IssueModel issue)
	{

		Id = issue.Id;
		Title = issue.Title;
		Description = issue.Description;
		Author = issue.Author;

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentSourceModel"/> class.
	/// </summary>
	/// <param name="solution">The solution.</param>
	public BasicCommentSourceModel(SolutionModel solution)
	{

		Id = solution.Id;
		Title = solution.Title;
		Description = solution.Description;
		Author = solution.Author;

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentSourceModel"/> class.
	/// </summary>
	/// <param name="comment">The comment.</param>
	public BasicCommentSourceModel(CommentModel comment)
	{

		Id = comment.Id;
		Title = string.Empty;
		Description = comment.Comment;
		Author = comment.Author;

	}

	public string Id { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public BasicUserModel Author { get; set; } = new();

}
