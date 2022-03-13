using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public class IssueRepository : IIssueRepository
{
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<Issue> _collection;
	private readonly IMongoCollection<User> _userCollection;

	public IssueRepository(IMongoDbContext context)
	{
		_context = context;
		_collection = context.GetCollection<Issue>(GetCollectionName(nameof(Issue)));
		_userCollection = context.GetCollection<User>(GetCollectionName(nameof(User)));
	}

	public async Task CreateIssue(Issue issue)
	{
		using var session = await _context.Client.StartSessionAsync();
		
		session.StartTransaction();

		try
		{
			var issuesInTransaction = _collection;
			
			await issuesInTransaction.InsertOneAsync(issue);

			var usersInTransaction = _userCollection;
			
			var user = (await _userCollection.FindAsync(u => u.Id == issue.Author.Id)).First();
			
			user.AuthoredIssues.Add(new BasicIssueModel(issue));
			
			await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

			await session.CommitTransactionAsync();
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}

	public async Task<Issue> GetIssue(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<Issue>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<List<Issue>> GetIssues()
	{
		var all = await _collection.FindAsync(Builders<Issue>.Filter.Empty);

		return await all.ToListAsync();
	}

	public async Task<List<Issue>> GetUsersIssues(string userId)
	{
		var objectId = new ObjectId(userId);

		var results = await _collection.FindAsync(s => s.Author.Id == objectId.ToString());

		return results.ToList();
	}

	public async Task UpdateIssue(string id, Issue issue)
	{
		var objectId = new ObjectId(id);

		await _collection.ReplaceOneAsync(Builders<Issue>.Filter.Eq("_id", objectId), issue);
	}
}