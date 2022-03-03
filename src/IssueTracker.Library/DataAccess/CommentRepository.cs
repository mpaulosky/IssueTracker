using static IssueTrackerLibrary.Helpers.CollectionNames;

namespace IssueTrackerLibrary.DataAccess;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
	private readonly IMongoCollection<Comment> _collection;
	public CommentRepository(IMongoDbContext context) : base(context)
	{
		_collection = context.GetCollection<Comment>(GetCollectionName(nameof(Comment)));
	}
}