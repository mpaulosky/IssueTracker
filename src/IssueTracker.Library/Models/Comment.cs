namespace IssueTrackerLibrary.Models;

[Serializable]
public class Comment
{
	[BsonId, BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	
	[BsonElement("comment_name"), BsonRepresentation(BsonType.String)]
	public string CommentName { get; set; }
	
	[BsonElement("date_created"), BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	
	public BasicUserModel Author { get; set; }
	
	public HashSet<string> UserVotes { get; set; } = new();
	
	public Status IssueStatus { get; set; }
	
	[BsonElement("archived"), BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; } = false;
}