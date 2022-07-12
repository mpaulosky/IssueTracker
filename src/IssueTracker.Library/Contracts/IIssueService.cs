namespace IssueTracker.Library.Contracts;

public interface IIssueService
{
	Task CreateIssue(IssueModel suggestion);
	
	Task<IssueModel> GetIssue(string id);
	
	Task<List<IssueModel>> GetIssues();
	
	Task<List<IssueModel>> GetUsersIssues(string userId);
	
	Task UpdateIssue(IssueModel suggestion);
}