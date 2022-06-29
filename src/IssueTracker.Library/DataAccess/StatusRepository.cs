using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class StatusRepository : IStatusRepository
{
	private readonly IMongoCollection<Status> _collection;

	public StatusRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<Status>(GetCollectionName(nameof(Status)));
	}

	public async Task<Status> GetStatus(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<Status>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<Status>> GetStatuses()
	{
		var all = await _collection.FindAsync(Builders<Status>.Filter.Empty);
		
		return await all.ToListAsync();
	}

	public async Task CreateStatus(Status status)
	{
		await _collection.InsertOneAsync(status);
	}

	public async Task UpdateStatus(string id, Status status)
	{
		await _collection.ReplaceOneAsync(Builders<Status>.Filter.Eq("_id", id), status);
	}
}