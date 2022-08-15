namespace IssueTracker.Library.Services;

/// <summary>
/// StatusService class
/// </summary>
public class StatusService : IStatusService
{
	private const string _cacheName = "StatusData";
	private readonly IMemoryCache _cache;
	private readonly IStatusRepository _repository;

	/// <summary>
	/// StatusService constructor
	/// </summary>
	/// <param name="repository">IStatusRepository</param>
	/// <param name="cache">IMemoryCache</param>
	public StatusService(IStatusRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	/// CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateStatus(StatusModel status)
	{
		if (status == null)
		{
			throw new ArgumentNullException(nameof(status));
		}

		return _repository.CreateStatus(status);
	}

	/// <summary>
	/// GetStatus method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task StatusModel</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<StatusModel> GetStatus(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var result = await _repository.GetStatus(id);

		return result;
	}

	/// <summary>
	/// GetStatuses method
	/// </summary>
	/// <returns>Task of List StatusModels</returns>
	public async Task<List<StatusModel>> GetStatuses()
	{
		var output = _cache.Get<List<StatusModel>>(_cacheName);
		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetStatuses();
		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	/// <summary>
	/// UpdateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateStatus(StatusModel status)
	{
		if (status == null)
		{
			throw new ArgumentNullException(nameof(status));
		}

		return _repository.UpdateStatus(status.Id, status);
	}
}