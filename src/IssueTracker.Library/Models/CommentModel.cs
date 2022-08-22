//-----------------------------------------------------------------------
// <copyright file="CommentModel.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Models;

[Serializable]
public class CommentModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	public BasicIssueModel Issue { get; set; }

	[BsonElement("comment")]
	[BsonRepresentation(BsonType.String)]
	public string Comment { get; set; }

	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	public BasicUserModel Author { get; set; }

	public HashSet<string> UserVotes { get; set; } = new();

	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; }
}