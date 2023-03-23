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
	public BasicCommentModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentModel"/> class.
	/// </summary>
	/// <param name="comment">The comment.</param>
	public BasicCommentModel(CommentModel comment)
	{

		Id = comment.Id;
		Comment = comment.Comment;

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicCommentModel"/> class.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="comment">The comment.</param>
	public BasicCommentModel(string id, string comment) : this()
	{

		Id = id;
		Comment = comment;

	}

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the comment.
	/// </summary>
	/// <value>
	/// The comment.
	/// </value>
	public string Comment { get; set; } = string.Empty;

}