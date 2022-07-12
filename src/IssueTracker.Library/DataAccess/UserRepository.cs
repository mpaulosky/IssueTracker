using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class UserRepository : IUserRepository
{
	private readonly IMongoCollection<UserModel> _collection;

	public UserRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));
	}

	public async Task<UserModel> GetUser(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<UserModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<UserModel>> GetUsers()
	{
		var all = await _collection.FindAsync(Builders<UserModel>.Filter.Empty);
		
		return await all.ToListAsync();
	}

	public async Task CreateUser(UserModel user)
	{
		await _collection.InsertOneAsync(user);
	}

	public async Task UpdateUser(string id, UserModel user)
	{
		await _collection.ReplaceOneAsync(Builders<UserModel>.Filter.Eq("_id", id), user);
	}

	public async Task<UserModel> GetUserFromAuthentication(string objectId)
	{
		var results = await _collection.FindAsync(u => u.ObjectIdentifier == objectId);
		
		return results.FirstOrDefault();
	}
}