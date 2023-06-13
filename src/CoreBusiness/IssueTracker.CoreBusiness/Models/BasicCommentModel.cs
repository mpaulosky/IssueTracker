// Copyright (c) 2023. All rights reserved.
// File Name :     BasicCommentModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   BasicCommentModel class
/// </summary>
[Serializable]
public class BasicCommentModel
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicCommentModel" /> class.
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

	public string Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTime DateCreated { get; set; }
	public BasicCommentOnSourceModel CommentOnSource { get; set; }
	public BasicUserModel Author { get; set; }
}