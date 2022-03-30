namespace IssueTracker.Library.Models;

public class BasicIssueModel
{
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	public string Issue { get; set; }

	public BasicIssueModel()
	{
		
	}
	
	public BasicIssueModel(Issue issue)
	{
		Id = issue.Id;
		Issue = issue.IssueName;
	}
}