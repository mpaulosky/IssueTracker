namespace IssueTrackerLibrary.Contracts;

public interface IIssueRepository
{
	Task CreateIssue(Issue issue);
	Task<Issue> GetIssue(string id);
	Task<IEnumerable<Issue>> GetIssues();
	Task<IEnumerable<Issue>> GetUsersIssues(string userId);
	Task UpdateIssue(string id, Issue issue);
}