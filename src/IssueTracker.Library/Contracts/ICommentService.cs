namespace IssueTracker.Library.Contracts;

public interface ICommentService
{
	Task CreateComment(Comment comment);
	
	Task<Comment> GetComment(string id);
	
	Task<List<Comment>> GetComments();
	
	Task<List<Comment>> GetUsersComments(string userId);
	
	Task UpdateComment(Comment comment);
	
	Task UpvoteComment(string commentId, string userId);
}