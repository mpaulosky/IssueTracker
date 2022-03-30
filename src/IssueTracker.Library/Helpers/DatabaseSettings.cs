namespace IssueTracker.Library.Helpers;

public class DatabaseSettings : IDatabaseSettings
{
	public string ConnectionString { get; set; } = null!;

	public string DatabaseName { get; set; } = null!;
}