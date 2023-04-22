//-----------------------------------------------------------------------
// <copyright>
//	File:		DatabaseSettings.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Helpers;

/// <summary>
///		DatabaseSettings class
/// </summary>
public class DatabaseSettings
{

	public DatabaseSettings(string connectionString, string databaseName)
	{

		ConnectionString = connectionString;
		DatabaseName = databaseName;

	}

	public string ConnectionString { get; }

	public string DatabaseName { get; }

}
