using IssueTrackerLibrary.Contracts;

namespace IssueTrackerLibrary.DataAccess;

public class UserRepository : BaseRepository<User>, IUserRepository
{
	private readonly IMongoCollection<User> _collection;

	public UserRepository(IMongoDbContext context) : base(context)
	{
		_collection = context.GetCollection<User>("User");
	}
	
	public async Task<User> GetUserFromAuthentication(string objectId)
	{
		var results = await _collection.FindAsync(u => u.ObjectIdentifier == objectId);
		
		return results.FirstOrDefault();
	}
}