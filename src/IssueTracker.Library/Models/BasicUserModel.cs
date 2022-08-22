//-----------------------------------------------------------------------
// <copyright file="BasicUserModel.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Models;

public class BasicUserModel
{
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

	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	public string DisplayName { get; set; }
}