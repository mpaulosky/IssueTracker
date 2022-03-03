using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	private readonly IMongoCollection<TEntity> _collection;

	protected BaseRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<TEntity>(GetCollectionName(typeof(TEntity).Name));
	}

	public async Task<TEntity> Get(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<TEntity>.Filter.Eq("_id", objectId);

		return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<TEntity>> Get()
	{
		var all = await _collection.FindAsync(Builders<TEntity>.Filter.Empty);
		return await all.ToListAsync();
	}

	public async Task Create(TEntity obj)
	{
		await _collection.InsertOneAsync(obj);
	}

	public async Task Update(string id, TEntity obj)
	{
		await  _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", id), obj);
	}
}