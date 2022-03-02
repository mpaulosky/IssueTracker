using IssueTrackerLibrary.Contracts;

using static IssueTrackerLibrary.Models.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public class IssueRepository : BaseRepository<Issue>
{
	private readonly IMongoCollection<Issue> _collection;

	public IssueRepository(IMongoDbContext context) : base(context)
	{
		_collection = context.GetCollection<Issue>(GetCollectionName(nameof(Issue)));
	}
}