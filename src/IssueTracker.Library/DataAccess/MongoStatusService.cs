using IssueTrackerLibrary.Contracts;

using Microsoft.Extensions.Caching.Memory;

using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public class MongoStatusService : IStatusService
{
	private readonly IMongoCollection<Status> _statuses;
	private readonly IMemoryCache _cache;
	private const string _cacheName = "StatusData";

	public MongoStatusService(IMongoDbContext db, IMemoryCache cache)
	{
		_statuses = db.GetCollection<Status>(GetCollectionName(nameof(Status)));
		_cache = cache;
	}

	public async Task<List<Status>> GetAllStatuses()
	{
		var output = _cache.Get<List<Status>>(_cacheName);
		if (output is null)
		{
			var results = await _statuses.FindAsync(_ => true);
			output = results.ToList();

			_cache.Set(_cacheName, output, TimeSpan.FromDays(1));
		}

		return output;
	}

	public Task CreateStatus(Status status)
	{
		return _statuses.InsertOneAsync(status);
	}
}