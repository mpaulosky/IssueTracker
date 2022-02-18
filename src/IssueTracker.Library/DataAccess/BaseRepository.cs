using IssueTrackerLibrary.Contracts;

namespace IssueTrackerLibrary.DataAccess;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<TEntity> _collection;

	protected BaseRepository(IMongoDbContext context)
	{
		_context = context;
		_collection = context.GetCollection<TEntity>(typeof(TEntity).Name);
	}

	public async Task<TEntity> Get(string id)
	{
		var objectId = new ObjectId(id);

		FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);

		return await _collection.FindAsync(filter).Result.FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<TEntity>> Get()
	{
		var all = await _collection.FindAsync(Builders<TEntity>.Filter.Empty);
		return await all.ToListAsync();
	}

	public async Task Create(TEntity obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
		}

		await _collection.InsertOneAsync(obj);
	}

	public async Task Update(string id, TEntity obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj));
		}

		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		await  _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", id), obj);
	}

	// public void Delete(string id)
	// {
	// 	var objectId = new ObjectId(id);
	// 	_collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
	// }
}