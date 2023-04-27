//-----------------------------------------------------------------------
// <copyright>
//	File:		BasicCommentModel.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
/// BasicCommentModel class
/// </summary>
[Serializable]
public class BasicCommentModel
{

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentModel"/> class.
	/// </summary>
	/// <param name="comment">The comment.</param>
	public BasicCommentModel(CommentModel comment)
	{

		Id = comment.Id;
		Title = comment.Title;
		Description = comment.Description;
		DateCreated = comment.DateCreated;
		CommentOnSource = comment.CommentOnSource!;
		Author = comment.Author;

	}

	public string Id { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public BasicCommentOnSourceModel CommentOnSource { get; set; }
	public BasicUserModel Author { get; set; } = new();

}
