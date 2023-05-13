namespace IssueTracker.CoreBusiness.Helpers;

[ExcludeFromCodeCoverage]
public class DatabaseSettingsTests
{

	private static DatabaseSettings CreateDatabaseSettings(string expectedCs, string expectedDbName)
	{
		return new DatabaseSettings(expectedCs, expectedDbName);
	}

	[Fact]
	public void TestMethod1()
	{

		// Arrange
		const string expectedCs = "ConnectionString";
		const string expectedDbName = "DatabaseName";

		// Act
		var databaseSettings = CreateDatabaseSettings(expectedCs, expectedDbName);


		// Assert
		databaseSettings.ConnectionString.Should().Be(expectedCs);
		databaseSettings.DatabaseName.Should().Be(expectedDbName);

	}

}
