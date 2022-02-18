namespace IssueTrackerLibrary.Models;

public class Comment
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string CommentName { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public BasicUserModel Author { get; set; }
	public HashSet<string> UserVotes { get; set; } = new();
	public Status IssueStatus { get; set; }
	public bool Archived { get; set; } = false;
}