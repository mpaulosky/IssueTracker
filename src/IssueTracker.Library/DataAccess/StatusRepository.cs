using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class StatusRepository : IStatusRepository
{
	private readonly IMongoCollection<StatusModel> _collection;

	public StatusRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<StatusModel>(GetCollectionName(nameof(StatusModel)));
	}

	public async Task<StatusModel> GetStatus(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<StatusModel>> GetStatuses()
	{
		var all = await _collection.FindAsync(Builders<StatusModel>.Filter.Empty);

		return await all.ToListAsync();
	}

	public async Task CreateStatus(StatusModel status)
	{
		await _collection.InsertOneAsync(status);
	}

	public async Task UpdateStatus(string id, StatusModel status)
	{
		await _collection.ReplaceOneAsync(Builders<StatusModel>.Filter.Eq("_id", id), status);
	}
}