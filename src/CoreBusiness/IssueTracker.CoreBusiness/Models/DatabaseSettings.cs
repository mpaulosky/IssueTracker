//-----------------------------------------------------------------------
// <copyright>
//	File:		DatabaseSettings.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   DatabaseSettings class
/// </summary>
public class DatabaseSettings : IDatabaseSettings
{
	public DatabaseSettings()
	{
	}

	public DatabaseSettings(string connectionStrings, string databaseName)
	{
		ConnectionStrings = connectionStrings;
		DatabaseName = databaseName;
	}

	public string ConnectionStrings { get; set; } = null!;

	public string DatabaseName { get; set; } = null!;
}
