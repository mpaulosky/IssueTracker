namespace IssueTracker.Library.Contracts;

public interface ICommentRepository
{
	Task<CommentModel> GetComment(string id);

	Task<IEnumerable<CommentModel>> GetComments();

	Task UpdateComment(string id, CommentModel obj);

	Task<IEnumerable<CommentModel>> GetUsersComments(string userId);

	Task CreateComment(CommentModel comment);

	Task UpvoteComment(string commentId, string userId);
}