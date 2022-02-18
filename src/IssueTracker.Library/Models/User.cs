namespace IssueTrackerLibrary.Models;

public class User
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; init; }
	public string ObjectIdentifier { get; init; }
	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string DisplayName { get; init; }
	public string EmailAddress { get; init; }
	public List<BasicIssueModel> AuthoredIssues { get; init; } = new();
	public List<BasicCommentModel> VotedOnComments { get; init; } = new();
	public List<BasicCommentModel> AuthoredComments { get; init; } = new();
}