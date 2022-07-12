﻿namespace IssueTracker.Library.Models;

public class BasicCommentModel
{
	public BasicCommentModel()
	{
	}

	public BasicCommentModel(CommentModel comment)
	{
		Id = comment.Id;
		Comment = comment.CommentName;
	}

	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	public string Comment { get; set; }
}