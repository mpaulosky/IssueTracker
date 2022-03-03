using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public class IssueRepository : BaseRepository<Issue>, IIssueRepository
{
	private readonly IMongoCollection<Issue> _collection;

	public IssueRepository(IMongoDbContext context) : base(context)
	{
		_collection = context.GetCollection<Issue>(GetCollectionName(nameof(Issue)));
	}

	public async Task<List<Issue>> GetUsersIssues(string userId)
	{
		var results = await _collection.FindAsync(s => s.Author.Id == userId);
		return results.ToList();
	}
}