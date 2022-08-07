using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class CommentRepository : ICommentRepository
{
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<CommentModel> _commentCollection;
	private readonly IMongoCollection<UserModel> _userCollection;

	public CommentRepository(IMongoDbContext context)
	{
		_context = context;
		_commentCollection = context.GetCollection<CommentModel>(GetCollectionName(nameof(CommentModel)));
		_userCollection = context.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));
	}

	public async Task CreateComment(CommentModel comment)
	{
		using var session = await _context.Client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			var commentsInTransaction = _commentCollection ?? throw new ArgumentNullException(nameof(_commentCollection));

			await commentsInTransaction.InsertOneAsync(comment);

			var usersInTransaction = _userCollection ?? throw new ArgumentNullException(nameof(_userCollection));

			var user = (await _userCollection.FindAsync(u => u.Id == comment.Author.Id)).First();

			user.AuthoredComments.Add(new BasicCommentModel(comment));

			await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

			await session.CommitTransactionAsync();
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}

	public async Task<CommentModel> GetComment(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<CommentModel>.Filter.Eq("_id", objectId);

		var result = await _commentCollection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<CommentModel>> GetComments()
	{
		var all = await _commentCollection.FindAsync(Builders<CommentModel>.Filter.Empty);

		return await all.ToListAsync();
	}

	public async Task<IEnumerable<CommentModel>> GetUsersComments(string userId)
	{
		var objectId = new ObjectId(userId);

		var results = await _commentCollection.FindAsync(s => s.Author.Id == objectId.ToString());

		return await results.ToListAsync();
	}

	public async Task<IEnumerable<CommentModel>> GetIssuesComments(string issueId)
	{
		var objectId = new ObjectId(issueId);

		var results = await _commentCollection.FindAsync(s => s.Issue.Id == objectId.ToString());

		return await results.ToListAsync();
	}

	public async Task UpdateComment(string id, CommentModel comment)
	{
		var objectId = new ObjectId(id);

		await _commentCollection.ReplaceOneAsync(Builders<CommentModel>.Filter.Eq("_id", objectId), comment);
	}

	public async Task UpvoteComment(string commentId, string userId)
	{

		using var session = await _context.Client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			var commentsInTransaction = _commentCollection;
			
			var objectId = new ObjectId(commentId);

			var filterComment = Builders<CommentModel>.Filter.Eq("_id", objectId);

			var comment = (await commentsInTransaction.FindAsync(filterComment)).FirstOrDefault();
			
			bool isUpvote = comment.UserVotes.Add(userId);

			if (isUpvote == false)
			{
				comment.UserVotes.Remove(userId);
			}

			await commentsInTransaction.ReplaceOneAsync(session, s => s.Id == commentId, comment);

			await session.CommitTransactionAsync();
			
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}
}