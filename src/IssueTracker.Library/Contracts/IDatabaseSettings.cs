namespace IssueTracker.Library.Contracts;

public interface IDatabaseSettings
{
	string ConnectionString { get; set; }
	string DatabaseName { get; set; }
}