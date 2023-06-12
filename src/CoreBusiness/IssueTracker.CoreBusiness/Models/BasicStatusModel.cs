// Copyright (c) 2023. All rights reserved.
// File Name :     BasicStatusModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   BasicStatusModel class
/// </summary>
[Serializable]
public class BasicStatusModel
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicStatusModel" /> class.
	/// </summary>
	public BasicStatusModel()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicStatusModel" /> class.
	/// </summary>
	/// <param name="status">The status.</param>
	public BasicStatusModel(StatusModel status)
	{
		StatusName = status.StatusName;
		StatusDescription = status.StatusDescription;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicStatusModel" /> class.
	/// </summary>
	/// <param name="statusName">Name of the status.</param>
	/// <param name="statusDescription">The status description.</param>
	public BasicStatusModel(string statusName, string statusDescription) : this()
	{
		StatusName = statusName;
		StatusDescription = statusDescription;
	}

	/// <summary>
	///   Gets the name of the status.
	/// </summary>
	/// <value>
	///   The name of the status.
	/// </value>
	public string StatusName { get; init; } = string.Empty;

	/// <summary>
	///   Gets the status description.
	/// </summary>
	/// <value>
	///   The status description.
	/// </value>
	public string StatusDescription { get; init; } = string.Empty;
}
