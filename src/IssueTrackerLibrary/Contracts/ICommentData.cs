namespace IssueTrackerLibrary.DataAccess;

public interface ICommentData
{
	Task<List<CommentModel>> GetAllComments();
	Task<List<CommentModel>> GetUsersComments(string userId);
	Task<List<CommentModel>> GetAllApprovedComments();
	Task<CommentModel> GetComment(string id);
	Task<List<CommentModel>> GetAllCommentsWaitingForApproval();
	Task UpdateComment(CommentModel suggestion);
	Task UpvoteComment(string commentId, string userId);
	Task CreateComment(CommentModel comment);
}