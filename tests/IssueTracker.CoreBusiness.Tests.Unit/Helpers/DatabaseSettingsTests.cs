namespace IssueTracker.CoreBusiness.Helpers;

[ExcludeFromCodeCoverage]
public class DatabaseSettingsTests
{

	private static DatabaseSettings CreateDatabaseSettings(string expectedCs, string expectedDbName)
	{
		return new DatabaseSettings(expectedCs, expectedDbName)
		{
			ConnectionStrings = expectedCs,
			DatabaseName = expectedDbName
		};
	}

	[Fact(DisplayName = "CreateDatabaseSettings")]
	public void CreateDatabaseSettings_With_Valid_Data_Should_Be_Successful_Test()
	{

		// Arrange
		const string expectedCs = "ConnectionString";
		const string expectedDbName = "DatabaseName";

		// Act
		var databaseSettings = CreateDatabaseSettings(expectedCs, expectedDbName);


		// Assert
		databaseSettings.ConnectionStrings.Should().Be(expectedCs);
		databaseSettings.DatabaseName.Should().Be(expectedDbName);

	}

}
