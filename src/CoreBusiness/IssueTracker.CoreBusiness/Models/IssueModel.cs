//-----------------------------------------------------------------------
// <copyright file="IssueModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class IssueModel
{

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	[BsonElement("issue_title")]
	[BsonRepresentation(BsonType.String)]
	public string IssueTitle { get; set; } = string.Empty;

	[BsonElement("description")]
	[BsonRepresentation(BsonType.String)]
	public string Description { get; set; } = string.Empty;

	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	public BasicCategoryModel Category { get; set; } = new();

	public BasicUserModel Author { get; set; } = new();

	public BasicStatusModel IssueStatus { get; set; } = new();

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