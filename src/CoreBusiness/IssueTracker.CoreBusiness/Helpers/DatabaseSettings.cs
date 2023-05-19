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
public class DatabaseSettings : IDatabaseSettings
{

	public DatabaseSettings(string connectionString, string databaseName)
	{

		ConnectionString = connectionString;
		DatabaseName = databaseName;

	}

	public required string ConnectionString { get; init;}

	public required string DatabaseName { get; init; }

}

public interface IDatabaseSettings
{

	string ConnectionString { get; init; }

	string DatabaseName { get; init; }

}
