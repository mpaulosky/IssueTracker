//-----------------------------------------------------------------------
// <copyright file="StatusModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class StatusModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	[BsonElement("status_name")]
	[BsonRepresentation(BsonType.String)]
	public string? StatusName { get; set; }

	[BsonElement("status_description")]
	[BsonRepresentation(BsonType.String)]
	public string? StatusDescription { get; set; }
}