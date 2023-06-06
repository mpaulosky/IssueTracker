//-----------------------------------------------------------------------// <copyright>//	File:		BasicUserModel.cs//	Company:mpaulosky//	Author:	Matthew Paulosky//	Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   BasicUserModel class
/// </summary>
[Serializable]public class BasicUserModel{
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicUserModel" /> class.
	/// </summary>
	public BasicUserModel()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicUserModel" /> class.
	/// </summary>
	/// <param name="user">The user.</param>
	public BasicUserModel(UserModel user)
	{
		Id = user.Id;
		FirstName = user.FirstName;
		LastName = user.LastName;
		EmailAddress = user.EmailAddress;
		DisplayName = user.DisplayName;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicUserModel" /> class.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <param name="displayName">The display name.</param>
	public BasicUserModel(
		string id,
		string firstName,
		string lastName,
		string emailAddress,
		string displayName) : this()
	{
		Id = id;
		FirstName = firstName;
		LastName = lastName;
		EmailAddress = emailAddress;
		DisplayName = displayName;
	}

	/// <summary>
	///   Gets the identifier.
	/// </summary>
	/// <value>
	///   The identifier.
	/// </value>
	public string Id { get; init; } = string.Empty;

	/// <summary>
	///   Gets or sets the first name.
	/// </summary>
	/// <value>
	///   The first name.
	/// </value>
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the last name.
	/// </summary>
	/// <value>
	///   The last name.
	/// </value>
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the display name.
	/// </summary>
	/// <value>
	///   The display name.
	/// </value>
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the email address.
	/// </summary>
	/// <value>
	///   The email address.
	/// </value>
	public string EmailAddress { get; set; } = string.Empty;}
