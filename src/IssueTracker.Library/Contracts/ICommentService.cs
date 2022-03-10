namespace IssueTrackerLibrary.Contracts;

public interface ICommentService
{
	Task<List<Comment>> GetAllComments();
	Task<List<Comment>> GetUsersComments(string userId);
	Task<Comment> GetComment(string id);
	Task Create(Comment comment);
	Task Update(Comment suggestion);
	Task UpvoteComment(string commentId, string userId);
}