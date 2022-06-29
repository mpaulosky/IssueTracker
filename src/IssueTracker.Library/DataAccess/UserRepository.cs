using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class UserRepository : IUserRepository
{
	private readonly IMongoCollection<User> _collection;

	public UserRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<User>(GetCollectionName(nameof(User)));
	}

	public async Task<User> GetUser(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<User>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<User>> GetUsers()
	{
		var all = await _collection.FindAsync(Builders<User>.Filter.Empty);
		
		return await all.ToListAsync();
	}

	public async Task CreateUser(User user)
	{
		await _collection.InsertOneAsync(user);
	}

	public async Task UpdateUser(string id, User user)
	{
		await _collection.ReplaceOneAsync(Builders<User>.Filter.Eq("_id", id), user);
	}

	public async Task<User> GetUserFromAuthentication(string objectId)
	{
		var results = await _collection.FindAsync(u => u.ObjectIdentifier == objectId);
		
		return results.FirstOrDefault();
	}
}