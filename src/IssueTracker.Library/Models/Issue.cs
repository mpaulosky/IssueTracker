namespace IssueTrackerLibrary.Models;

public class Issue
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string IssueName { get; set; }
	public string Description { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public BasicUserModel Author { get; set; }
	public Status IssueStatus { get; set; }
	public string OwnerNotes { get; set; }
	public bool Archived { get; set; } = false;
}