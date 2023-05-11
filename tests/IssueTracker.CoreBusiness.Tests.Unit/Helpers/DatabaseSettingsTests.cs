namespace IssueTracker.CoreBusiness.Helpers;

[ExcludeFromCodeCoverage]
public class DatabaseSettingsTests
{

	private DatabaseSettings CreateDatabaseSettings(string expectedCs, string expectedDbName)
	{
		return new DatabaseSettings(expectedCs, expectedDbName);
	}

	[Fact]
	public void TestMethod1()
	{

		// Arrange
		const string expectedCS = "ConnectionString";
		const string expectedDbName = "DatabaseName";

		// Act
		var databaseSettings = CreateDatabaseSettings(expectedCS, expectedDbName);


		// Assert
		databaseSettings.ConnectionString.Should().Be(expectedCS);
		databaseSettings.DatabaseName.Should().Be(expectedDbName);

	}

}
