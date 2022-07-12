namespace IssueTracker.Library.Contracts;

public interface IStatusService
{
	Task CreateStatus(StatusModel status);
	
	Task<StatusModel> GetStatus(string id);
	
	Task<List<StatusModel>> GetStatuses();
	
	Task UpdateStatus(StatusModel status);
}