using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public class StatusRepository : BaseRepository<Status>, IStatusRepository
{
	private readonly IMongoCollection<Status> _collection;

	public StatusRepository(IMongoDbContext context) : base(context)
	{
		_collection = context.GetCollection<Status>(GetCollectionName(nameof(Status)));
	}
}