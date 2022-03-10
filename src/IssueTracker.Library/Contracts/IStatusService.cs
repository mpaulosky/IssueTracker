namespace IssueTrackerLibrary.Contracts;

public interface IStatusService
{
	Task<Status> GetStatus(string id);
	Task<List<Status>> GetAllStatuses();
	Task CreateStatus(Status status);
	Task UpdateStatus(string id, Status status);
}