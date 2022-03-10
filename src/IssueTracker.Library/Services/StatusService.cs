using Microsoft.Extensions.Caching.Memory;

using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.Services;

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

	public async Task<Status> GetStatus(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var results = await _repository.GetStatus(id);
		
		return results;
	}

	public async Task<List<Status>> GetAllStatuses()
	{
		var output = _cache.Get<List<Status>>(_cacheName);
		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetStatuses();
		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	public Task CreateStatus(Status status)
	{
		if (status == null)
		{
			throw new ArgumentNullException(nameof(status));
		}

		return _repository.CreateStatus(status);
	}

	public Task UpdateStatus(string id, Status status)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		if (status == null)
		{
			throw new ArgumentNullException(nameof(status));
		}

		return _repository.UpdateStatus(id, status);
	}
}