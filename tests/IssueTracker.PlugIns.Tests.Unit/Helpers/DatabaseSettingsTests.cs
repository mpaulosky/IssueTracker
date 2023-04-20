
namespace IssueTracker.PlugIns.Tests.Unit.Helpers;

[ExcludeFromCodeCoverage]
public class DatabaseSettingsTests
{

	private DatabaseSettings CreateDatabaseSettings(string expectedCS, string expectedDbName)
	{
		return new DatabaseSettings(expectedCS, expectedDbName);
	}

	[Fact]
	public void TestMethod1()
	{

		// Arrange
		var expectedCS = "ConnectionString";
		var expectedDbName = "DatabaseName";

		// Act
		var databaseSettings = CreateDatabaseSettings(expectedCS, expectedDbName);


		// Assert
		databaseSettings.ConnectionString.Should().Be(expectedCS);
		databaseSettings.DatabaseName.Should().Be(expectedDbName);

	}

}
