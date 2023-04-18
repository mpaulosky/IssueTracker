//-----------------------------------------------------------------------
// <copyright>
//	File:		BasicUserModel.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
/// BasicUserModel class
/// </summary>
[Serializable]
public class BasicUserModel
{

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicUserModel"/> class.
	/// </summary>
	public BasicUserModel()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicUserModel"/> class.
	/// </summary>
	/// <param name="user">The user.</param>
	public BasicUserModel(UserModel user)
	{

		Id = user.Id;
		DisplayName = user.DisplayName;

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BasicUserModel"/> class.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="displayName">The display name.</param>
	public BasicUserModel(string id, string displayName) : this()
	{

		Id = id!;
		DisplayName = displayName!;

	}

	/// <summary>
	/// Gets the identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	public string Id { get; init; } = string.Empty;

	/// <summary>
	/// Gets the display name.
	/// </summary>
	/// <value>
	/// The display name.
	/// </value>
	public string DisplayName { get; init; } = string.Empty;

}
