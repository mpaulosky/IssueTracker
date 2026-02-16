// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     DatabaseSettings.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

using System.ComponentModel.DataAnnotations;

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   DatabaseSettings class providing strongly-typed configuration for database connections.
/// </summary>
public class DatabaseSettings : IDatabaseSettings
{
	/// <summary>
	///   Initializes a new instance of the <see cref=\"DatabaseSettings\"/> class.
	/// </summary>
	public DatabaseSettings()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref=\"DatabaseSettings\"/> class with specified values.
	/// </summary>
	/// <param name=\"connectionStrings\">The database connection string.</param>
	/// <param name=\"databaseName\">The name of the database.</param>
	public DatabaseSettings(string connectionStrings, string databaseName)
	{
		ConnectionStrings = connectionStrings;
		DatabaseName = databaseName;
	}

	/// <summary>
	///   Gets or sets the database connection string.
	/// </summary>
	/// <value>
	///   The connection string used to connect to the database.
	/// </value>
	[Required(ErrorMessage = "Database connection string is required.")]
	public string ConnectionStrings { get; set; } = null!;

	/// <summary>
	///   Gets or sets the name of the database.
	/// </summary>
	/// <value>
	///   The name of the database to connect to.
	/// </value>
	[Required(ErrorMessage = "Database name is required.")]
	public string DatabaseName { get; set; } = null!;
}
