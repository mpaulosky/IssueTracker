namespace IssueTrackerLibrary.Models;

public class IssueModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Issue { get; set; }
	public string Description { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	public BasicUserModel Author { get; set; }
	public StatusModel IssueStatus { get; set; }
	public string OwnerNotes { get; set; }
	public bool Archived { get; set; } = false;
}