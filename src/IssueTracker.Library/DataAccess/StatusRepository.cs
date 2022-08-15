using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

/// <summary>
/// StatusRepository class
/// </summary>
public class StatusRepository : IStatusRepository
{
	private readonly IMongoCollection<StatusModel> _collection;

	/// <summary>
	/// StatusRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	public StatusRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<StatusModel>(GetCollectionName(nameof(StatusModel)));
	}

	/// <summary>
	/// GetStatus method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of StatusModel</returns>
	public async Task<StatusModel> GetStatus(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<StatusModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	/// <summary>
	/// GetStatuses method
	/// </summary>
	/// <returns>Task of IEnumerable StatusModel</returns>
	public async Task<IEnumerable<StatusModel>> GetStatuses()
	{
		var all = await _collection.FindAsync(Builders<StatusModel>.Filter.Empty);

		return await all.ToListAsync();
	}

	/// <summary>
	/// CreateStatus method
	/// </summary>
	/// <param name="status">StatusModel</param>
	public async Task CreateStatus(StatusModel status)
	{
		await _collection.InsertOneAsync(status);
	}

	/// <summary>
	/// UpdateStatus method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="status">StatusModel</param>
	public async Task UpdateStatus(string id, StatusModel status)
	{
		await _collection.ReplaceOneAsync(Builders<StatusModel>.Filter.Eq("_id", id), status);
	}
}