﻿namespace IssueTracker.Library.Models;

public class BasicUserModel
{
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string DisplayName { get; set; }

	public BasicUserModel()
	{
	}
	
	public BasicUserModel(UserModel user)
	{
		Id = user.Id;
		DisplayName = user.DisplayName;
	}

	public BasicUserModel(string id, string displayName) : this()
	{
		Id = id;
		DisplayName = displayName;
	}
}