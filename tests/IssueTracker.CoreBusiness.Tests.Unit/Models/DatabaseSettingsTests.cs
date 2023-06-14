// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     DatabaseSettingsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class DatabaseSettingsTests
{
	private static DatabaseSettings CreateDatabaseSettings(string expectedCs, string expectedDbName)
	{
		return new DatabaseSettings(expectedCs, expectedDbName)
		{
			ConnectionStrings = expectedCs, DatabaseName = expectedDbName
		};
	}

	[Fact(DisplayName = "CreateDatabaseSettings")]
	public void CreateDatabaseSettings_With_Valid_Data_Should_Be_Successful_Test()
	{
		// Arrange
		const string expectedCs = "ConnectionString";
		const string expectedDbName = "DatabaseName";

		// Act
		DatabaseSettings databaseSettings = CreateDatabaseSettings(expectedCs, expectedDbName);

		// Assert
		databaseSettings.ConnectionStrings.Should().Be(expectedCs);
		databaseSettings.DatabaseName.Should().Be(expectedDbName);
	}

	[Fact(DisplayName = "GetDatabaseSettings")]
	public void GetDatabaseSettings_With_Valid_Data_Should_Be_Successful_Test()
	{
		// Arrange
		const string expectedCs = "ConnectionString";
		const string expectedDbName = "DatabaseName";

		DatabaseSettings databaseSettings = CreateDatabaseSettings(expectedCs, expectedDbName);

		// Act
		string valConn = databaseSettings.ConnectionStrings;
		string valDbName = databaseSettings.DatabaseName;

		// Assert
		valConn.Should().Be(expectedCs);
		valDbName.Should().Be(expectedDbName);
	}
}