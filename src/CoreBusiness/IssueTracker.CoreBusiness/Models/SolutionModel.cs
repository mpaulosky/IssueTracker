//-----------------------------------------------------------------------
// <copyright File="SolutionModel.cs"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

public class SolutionModel
{

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	[BsonElement("solution_title")]
	[BsonRepresentation(BsonType.String)]
	public string SolutionTitle { get; set; } = string.Empty;

	[BsonElement("solution")]
	[BsonRepresentation(BsonType.String)]
	public string Solution { get; set; } = string.Empty;

	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	public BasicIssueModel Issue { get; set; } = new();

	public BasicUserModel Author { get; set; } = new();

	public HashSet<string> UserVotes { get; set; } = new();

	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; } = false;

}
