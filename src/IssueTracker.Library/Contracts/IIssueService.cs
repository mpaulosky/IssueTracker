namespace IssueTrackerLibrary.Contracts;

public interface IIssueService
{
	Task<List<Issue>> GetAllIssues();
	Task<List<Issue>> GetUsersIssues(string userId);
	Task<List<Issue>> GetAllApprovedIssues();
	Task<Issue> GetIssue(string id);
	Task<List<Issue>> GetAllIssuesWaitingForApproval();
	Task UpdateIssue(Issue suggestion);
	Task UpvoteIssue(string suggestionId, string userId);
	Task CreateIssue(Issue suggestion);
}