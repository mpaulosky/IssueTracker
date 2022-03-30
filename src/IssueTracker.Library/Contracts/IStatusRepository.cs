namespace IssueTracker.Library.Contracts;

public interface IStatusRepository
{
	Task<Status> GetStatus(string id);
	
	Task<IEnumerable<Status>> GetStatuses();
	
	Task CreateStatus(Status status);

	Task UpdateStatus(string id, Status status);
}