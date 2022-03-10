namespace IssueTrackerLibrary.Contracts;

public interface ICommentRepository
{
	public Task<Comment> GetComment(string id);
	
	public Task<IEnumerable<Comment>> GetComments();
	
	public Task UpdateComment(string id, Comment obj);
	
	Task<List<Comment>> GetUsersComments(string userId);
	
	Task CreateComment(Comment comment);
	
	Task UpvoteComment(string commentId, string userId);
}