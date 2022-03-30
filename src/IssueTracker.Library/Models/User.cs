namespace IssueTracker.Library.Models;

[Serializable]
public class User
{
	[BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }
	
	[BsonElement("object_identifier")]
	public string ObjectIdentifier { get; set; }
	
	[BsonElement("first_name"), BsonRepresentation(BsonType.String)]
	public string FirstName { get; set; }
	
	[BsonElement("last_name"), BsonRepresentation(BsonType.String)]
	public string LastName { get; set; }
	
	[BsonElement("display_name"), BsonRepresentation(BsonType.String)]
	public string DisplayName { get; set; }
	
	[BsonElement("email_address"), BsonRepresentation(BsonType.String)]
	public string EmailAddress { get; set; }
	
	public List<BasicIssueModel> AuthoredIssues { get; set; } = new();
	
	public List<BasicCommentModel> VotedOnComments { get; set; } = new();
	
	public List<BasicCommentModel> AuthoredComments { get; set; } = new();
}