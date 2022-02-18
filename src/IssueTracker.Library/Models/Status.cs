﻿namespace IssueTrackerLibrary.Models;

public class Status
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string StatusName { get; set; }
	public string StatusDescription { get; set; }
}