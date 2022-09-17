//-----------------------------------------------------------------------
// <copyright file="IssueModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class IssueModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	[BsonElement("issue_name")]
	[BsonRepresentation(BsonType.String)]
	public string IssueName { get; set; }

	[BsonElement("description")]
	[BsonRepresentation(BsonType.String)]
	public string Description { get; set; }

	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	public BasicCategoryModel Category { get; set; }

	public BasicUserModel Author { get; set; }

	public BasicStatusModel IssueStatus { get; set; }

	[BsonElement("owner_notes")]
	[BsonRepresentation(BsonType.String)]
	public string OwnerNotes { get; set; }

	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; }

	[BsonElement("approved_for_release")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool ApprovedForRelease { get; set; }

	[BsonElement("rejected")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Rejected { get; set; }
}