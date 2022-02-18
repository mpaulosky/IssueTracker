namespace IssueTrackerLibrary.Contracts;

public interface ICommentService
{
	Task<List<Comment>> GetAllComments();
	Task<List<Comment>> GetUsersComments(string userId);
	Task<List<Comment>> GetAllApprovedComments();
	Task<Comment> GetComment(string id);
	Task<List<Comment>> GetAllCommentsWaitingForApproval();
	Task UpdateComment(Comment suggestion);
	Task UpvoteComment(string commentId, string userId);
	Task CreateComment(Comment comment);
}