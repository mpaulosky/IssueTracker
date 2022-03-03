﻿namespace IssueTrackerLibrary.Models;

[Serializable]
public class Status
{
	[BsonId, BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	
	[BsonElement("status_name"), BsonRepresentation(BsonType.String)]
	public string StatusName { get; set; }
	
	[BsonElement("status_description"), BsonRepresentation(BsonType.String)]
	public string StatusDescription { get; set; }
}