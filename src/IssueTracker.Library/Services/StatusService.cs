using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.Library.Services;

public class StatusService : IStatusService
{
	private readonly IStatusRepository _repository;
	private readonly IMemoryCache _cache;
	private const string _cacheName = "StatusData";

	public StatusService(IStatusRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	public Task CreateStatus(StatusModel status)
	{
		if (status == null)
		{
			throw new ArgumentNullException(nameof(status));
		}

		return _repository.CreateStatus(status);
	}

	public async Task<StatusModel> GetStatus(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var results = await _repository.GetStatus(id);
		
		return results;
	}

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

	public Task UpdateStatus(StatusModel status)
	{
		if (status == null)
		{
			throw new ArgumentNullException(nameof(status));
		}

		return _repository.UpdateStatus(status.Id, status);
	}
}