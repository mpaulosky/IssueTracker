namespace IssueTrackerLibrary.Models;

public class CommentModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Comment { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public BasicUserModel Author { get; set; }
	public HashSet<string> UserVotes { get; set; } = new();
	public StatusModel IssueStatus { get; set; }
	public bool Archived { get; set; } = false;
}