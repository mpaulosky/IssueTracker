using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class CommentRepository : ICommentRepository
{
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<Comment> _collection;
	private readonly IMongoCollection<User> _userCollection;
	
	public CommentRepository(IMongoDbContext context)
	{
		_context = context;
		_collection = context.GetCollection<Comment>(GetCollectionName(nameof(Comment)));
		_userCollection = context.GetCollection<User>(GetCollectionName(nameof(User)));
	}

	public async Task CreateComment(Comment comment)
	{
		using var session = await _context.Client.StartSessionAsync();
		
		session.StartTransaction();

		try
		{
			var commentsInTransaction = _collection;
			
			await commentsInTransaction.InsertOneAsync(comment);

			var usersInTransaction = _userCollection;
			
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

	public async Task<Comment> GetComment(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<Comment>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<Comment>> GetComments()
	{
		var all = await _collection.FindAsync(Builders<Comment>.Filter.Empty);
		
		return await all.ToListAsync();
	}

	public async Task UpdateComment(string id, Comment comment)
	{
		var objectId = new ObjectId(id);

		await _collection.ReplaceOneAsync(Builders<Comment>.Filter.Eq("_id", objectId), comment);
	}

	public async Task<IEnumerable<Comment>> GetUsersComments(string userId)
	{
		var objectId = new ObjectId(userId);

		var results = await _collection.FindAsync(s => s.Author.Id == objectId.ToString());
		
		return await results.ToListAsync();
	}

	public async Task UpvoteComment(string commentId, string userId)
	{
		using var session = await _context.Client.StartSessionAsync();

		session.StartTransaction();

		try
		{
			var commentsInTransaction = _collection;
			
			var comment = (await commentsInTransaction.FindAsync(s => s.Id == commentId)).First();

			bool isUpvote = comment.UserVotes.Add(userId);
			
			if (isUpvote == false)
			{
				comment.UserVotes.Remove(userId);
			}

			await commentsInTransaction.ReplaceOneAsync(session, s => s.Id == commentId, comment);

			var usersInTransaction = _userCollection;
			
			var user = (await _userCollection.FindAsync(u => u.Id == userId)).First();

			if (isUpvote)
			{
				user.VotedOnComments.Add(new BasicCommentModel(comment));
			}
			else
			{
				var commentToRemove = user.VotedOnComments.First(s => s.Id == commentId);
				user.VotedOnComments.Remove(commentToRemove);
			}

			await usersInTransaction.ReplaceOneAsync(session, u => u.Id == userId, user);

			await session.CommitTransactionAsync();
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}
}