namespace IssueTrackerLibrary.Models;

[Serializable]

public class User
{
	[BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; init; }
	
	[BsonElement("object_identifier"), BsonRepresentation(BsonType.ObjectId)]
	public string ObjectIdentifier { get; init; }
	
	[BsonElement("first_name"), BsonRepresentation(BsonType.String)]
	public string FirstName { get; init; }
	
	[BsonElement("last_name"), BsonRepresentation(BsonType.String)]
	public string LastName { get; init; }
	
	[BsonElement("display_name"), BsonRepresentation(BsonType.String)]
	public string DisplayName { get; init; }
	
	[BsonElement("email_address"), BsonRepresentation(BsonType.String)]
	public string EmailAddress { get; init; }
	
	public List<BasicIssueModel> AuthoredIssues { get; init; } = new();
	
	public List<BasicCommentModel> VotedOnComments { get; init; } = new();
	
	public List<BasicCommentModel> AuthoredComments { get; init; } = new();
}