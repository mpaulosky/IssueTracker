//-----------------------------------------------------------------------
// <copyright file="DatabaseSettings.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Helpers;

/// <summary>
/// DatabaseSettings class
/// </summary>
public class DatabaseSettings : IDatabaseSettings
{
	public string ConnectionString { get; set; } = null!;

	public string DatabaseName { get; set; } = null!;
}