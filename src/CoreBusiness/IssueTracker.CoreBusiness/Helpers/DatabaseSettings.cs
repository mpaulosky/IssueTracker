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

	public DatabaseSettings(string connectionStrings, string databaseName)
	{

		ConnectionStrings = connectionStrings;
		DatabaseName = databaseName;

	}

	public string ConnectionStrings { get; init; }

	public string DatabaseName { get; init; }

}

public interface IDatabaseSettings
{

	string ConnectionStrings { get; init; }

	string DatabaseName { get; init; }

}
