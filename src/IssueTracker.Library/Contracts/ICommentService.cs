namespace IssueTracker.Library.Contracts;

public interface ICommentService
{
	Task CreateComment(CommentModel comment);
	
	Task<CommentModel> GetComment(string id);
	
	Task<List<CommentModel>> GetComments();
	
	Task<List<CommentModel>> GetUsersComments(string userId);
	
	Task UpdateComment(CommentModel comment);
	
	Task UpvoteComment(string commentId, string userId);
}