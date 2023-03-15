//-----------------------------------------------------------------------
// <copyright file="CommentModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class CommentModel
{

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	public BasicIssueModel Issue { get; set; } = new BasicIssueModel();

	[BsonElement("comment")]
	[BsonRepresentation(BsonType.String)]
	public string Comment { get; set; } = string.Empty;

	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	public BasicUserModel Author { get; set; } = new();

	public HashSet<string> UserVotes { get; set; } = new();

	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; } = false;

}