namespace IssueTrackerLibrary.Models;

public class BasicCommentModel
{
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Comment { get; set; }

	public BasicCommentModel()
	{

	}

	public BasicCommentModel(CommentModel comment)
	{
		Id = comment.Id;
		Comment = comment.Comment;
	}
}