namespace IssueTrackerLibrary.DataAccess;

public interface IIssueData
{
	Task<List<IssueModel>> GetAllSuggestions();
	Task<List<IssueModel>> GetUsersSuggestions(string userId);
	Task<List<IssueModel>> GetAllApprovedSuggestions();
	Task<IssueModel> GetSuggestion(string id);
	Task<List<IssueModel>> GetAllSuggestionsWaitingForApproval();
	Task UpdateSuggestion(IssueModel suggestion);
	Task UpvoteSuggestion(string suggestionId, string userId);
	Task CreateSuggestion(IssueModel suggestion);
}