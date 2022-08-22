//-----------------------------------------------------------------------
// <copyright file="IssueModel.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
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

	public CategoryModel Category { get; set; }
	
	public BasicUserModel Author { get; set; }

	public StatusModel IssueStatus { get; set; }

	[BsonElement("owner_notes")]
	[BsonRepresentation(BsonType.String)]
	public string OwnerNotes { get; set; }

	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; } = false;
	public bool ApprovedForRelease { get; set; }
	public bool Rejected { get; set; }

}