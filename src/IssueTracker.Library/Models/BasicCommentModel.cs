//-----------------------------------------------------------------------
// <copyright File="BasicCommentModel.cs"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class BasicCommentModel
{
	public BasicCommentModel()
	{
	}

	public BasicCommentModel(CommentModel comment)
	{
		Id = comment.Id;
		Comment = comment.Comment;
	}

	public BasicCommentModel(string id, string comment) : this()
	{
		Id = id;
		Comment = comment;
	}

	public string Id { get; set; }

	public string Comment { get; set; }
}