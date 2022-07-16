namespace IssueTracker.Library.Contracts;

public interface IIssueRepository
{
	Task CreateIssue(IssueModel issue);
	Task<IssueModel> GetIssue(string id);
	Task<IEnumerable<IssueModel>> GetIssues();
	Task<IEnumerable<IssueModel>> GetUsersIssues(string userId);
	Task UpdateIssue(string id, IssueModel issue);
	Task<IEnumerable<IssueModel>> GetIssuesWaitingForApproval();
	Task<IEnumerable<IssueModel>> GetApprovedIssues();

}