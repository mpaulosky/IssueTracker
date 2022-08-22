namespace IssueTracker.Library.Contracts;

public interface ICommentRepository
{
	Task<CommentModel> GetComment(string id);

	Task<IEnumerable<CommentModel>> GetComments();

	Task UpdateComment(string id, CommentModel comment);

	Task<IEnumerable<CommentModel>> GetUsersComments(string userId);

	Task<IEnumerable<CommentModel>> GetIssuesComments(string issueId);

	Task CreateComment(CommentModel comment);

	Task UpVoteComment(string commentId, string userId);
}