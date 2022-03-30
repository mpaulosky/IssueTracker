namespace IssueTracker.Library.Contracts;

public interface IStatusService
{
	Task CreateStatus(Status status);
	
	Task<Status> GetStatus(string id);
	
	Task<List<Status>> GetStatuses();
	
	Task UpdateStatus(Status status);
}