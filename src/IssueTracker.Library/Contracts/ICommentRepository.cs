namespace IssueTrackerLibrary.Contracts;

public interface ICommentRepository
{
	Task<Comment> GetComment(string id);
	
	Task<IEnumerable<Comment>> GetComments();
	
	Task UpdateComment(string id, Comment obj);
	
	Task<IEnumerable<Comment>> GetUsersComments(string userId);
	
	Task CreateComment(Comment comment);
	
	Task UpvoteComment(string commentId, string userId);
}