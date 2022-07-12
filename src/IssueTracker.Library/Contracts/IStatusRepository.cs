namespace IssueTracker.Library.Contracts;

public interface IStatusRepository
{
	Task<StatusModel> GetStatus(string id);

	Task<IEnumerable<StatusModel>> GetStatuses();

	Task CreateStatus(StatusModel status);

	Task UpdateStatus(string id, StatusModel status);
}