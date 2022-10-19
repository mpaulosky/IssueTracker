//-----------------------------------------------------------------------
// <copyright file="BasicUserModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
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
		Id = id!;
		DisplayName = displayName!;
	}

	public string Id { get; init; }

	public string DisplayName { get; init; }
}