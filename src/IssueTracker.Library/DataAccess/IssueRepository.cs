using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class IssueRepository : IIssueRepository
{
	private readonly IMongoCollection<IssueModel> _collection;
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<UserModel> _userCollection;

	public IssueRepository(IMongoDbContext context)
	{
		_context = context;
		_collection = context.GetCollection<IssueModel>(GetCollectionName(nameof(IssueModel)));
		_userCollection = context.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));
	}

	public async Task CreateIssue(IssueModel issue)
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

	public async Task<IssueModel> GetIssue(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<IssueModel>> GetIssues()
	{
		var all = await _collection.FindAsync(Builders<IssueModel>.Filter.Empty);

		return await all.ToListAsync();
	}

	public async Task<IEnumerable<IssueModel>> GetIssuesWaitingForApproval()
	{
		var output = await GetIssues();
		return output.Where(x =>
			x.ApprovedForRelease == false
			&& x.Rejected == false).ToList();
	}

	public async Task<IEnumerable<IssueModel>> GetApprovedIssues()
	{
		var output = await GetIssues();
		return output.Where(x =>
			x.ApprovedForRelease == true
			&& x.Rejected == false).ToList();
	}


	public async Task<IEnumerable<IssueModel>> GetUsersIssues(string userId)
	{
		var objectId = new ObjectId(userId);

		var results = await _collection.FindAsync(s => s.Author.Id == objectId.ToString());

		return await results.ToListAsync();
	}

	public async Task UpdateIssue(string id, IssueModel issue)
	{
		var objectId = new ObjectId(id);

		await _collection.ReplaceOneAsync(Builders<IssueModel>.Filter.Eq("_id", objectId), issue);
	}
}