namespace IssueTrackerLibrary.Contracts;

public interface IIssueRepository
{
	Task CreateIssue(Issue issue);
	Task<Issue> GetIssue(string id);
	Task<List<Issue>> GetIssues();
	Task<List<Issue>> GetUsersIssues(string userId);
	Task UpdateIssue(string id, Issue issue);
}