﻿namespace IssueTrackerLibrary.Models;

public class BasicIssueModel
{
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Issue { get; set; }

	public BasicIssueModel()
	{

	}

	public BasicIssueModel(IssueModel issue)
	{
		Id = issue.Id;
		Issue = issue.Issue;
	}
}