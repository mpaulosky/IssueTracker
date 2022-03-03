namespace IssueTrackerLibrary.Contracts;

public interface IIssueRepository : IBaseRepository<Issue>
{
	Task<List<Issue>> GetUsersIssues(string userId);
}