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
public class BasicCommentOnSourceModel
{

	/// <summary>
	/// BasicCommentOnSourceModel constructor
	/// </summary>
	public BasicCommentOnSourceModel()
	{
		
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentOnSourceModel"/> class.
	/// </summary>
	/// <param name="issue">The issue.</param>
	public BasicCommentOnSourceModel(IssueModel issue)
	{

		Id = issue.Id;
		SourceType = "Issue";
		Title = issue.Title;
		Description = issue.Description;
		DateCreated = issue.DateCreated;
		Author = issue.Author;

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentOnSourceModel"/> class.
	/// </summary>
	/// <param name="solution">The solution.</param>
	public BasicCommentOnSourceModel(SolutionModel solution)
	{

		Id = solution.Id;
		SourceType = "Solution";
		Title = solution.Title;
		Description = solution.Description;
		DateCreated = solution.DateCreated;
		Author = solution.Author;

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentOnSourceModel"/> class.
	/// </summary>
	/// <param name="comment">The comment.</param>
	public BasicCommentOnSourceModel(CommentModel comment)
	{

		Id = comment.Id;
		SourceType = "Comment";
		Title = comment.Title;
		Description = comment.Description;
		DateCreated = comment.DateCreated;
		Author = comment.Author;

	}

	public string? Id { get; set; }
	public string? SourceType { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public BasicUserModel? Author { get; set; }

}
