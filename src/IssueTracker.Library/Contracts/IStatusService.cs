namespace IssueTrackerLibrary.Contracts;

public interface IStatusService
{
	Task<List<Status>> GetAllStatuses();
	Task CreateStatus(Status status);
}