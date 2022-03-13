namespace IssueTrackerLibrary.Contracts;

public interface IIssueService
{
	Task CreateIssue(Issue suggestion);
	Task<Issue> GetIssue(string id);
	Task<List<Issue>> GetIssues();
	Task<List<Issue>> GetUsersIssues(string userId);
	Task UpdateIssue(Issue suggestion);
}