//-----------------------------------------------------------------------
// <copyright file="DatabaseSettings.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Helpers;

/// <summary>
///		DatabaseSettings class
/// </summary>
public class DatabaseSettings : IDatabaseSettings
{

	public DatabaseSettings()
	{

	}

	public DatabaseSettings(string connectionString, string databaseName) : this()
	{

		ConnectionString = connectionString!;
		DatabaseName = databaseName!;

	}

	public string ConnectionString { get; set; }

	public string DatabaseName { get; set; }

}