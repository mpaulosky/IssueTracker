﻿//-----------------------------------------------------------------------
// <copyright>
//	File:		SolutionModel.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using JetBrains.Annotations;

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
/// SolutionModel class
/// </summary>
[Serializable]
public class SolutionModel
{

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the title.
	/// </summary>
	/// <value>
	/// The title.
	/// </value>
	[BsonElement("solution_title")]
	[BsonRepresentation(BsonType.String)]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description.
	/// </summary>
	/// <value>
	/// The description.
	/// </value>
	[BsonElement("solution_description")]
	[BsonRepresentation(BsonType.String)]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the date created.
	/// </summary>
	/// <value>
	/// The date created.
	/// </value>
	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the issue.
	/// </summary>
	/// <value>
	/// The issue.
	/// </value>
	public BasicIssueModel Issue { get; set; } = new();

	/// <summary>
	/// Gets or sets the author.
	/// </summary>
	/// <value>
	/// The author.
	/// </value>
	public BasicUserModel Author { get; set; } = new();

	/// <summary>
	/// Gets or sets the user votes.
	/// </summary>
	/// <value>
	/// The user votes.
	/// </value>
	public HashSet<string> UserVotes { get; set; } = new();

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="SolutionModel"/> is archived.
	/// </summary>
	/// <value>
	///   <c>true</c> if archived; otherwise, <c>false</c>.
	/// </value>
	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; }

}
