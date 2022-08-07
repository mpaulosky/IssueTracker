namespace IssueTracker.Library.Models;

public class BasicIssueModel
{
	public BasicIssueModel()
	{
	}

	public BasicIssueModel(IssueModel issue)
	{
		Id = issue.Id;
		Issue = issue.IssueName;
		Description = issue.Description;
	}

	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	public string Issue { get; set; }
	
	public string Description { get; set; }

}