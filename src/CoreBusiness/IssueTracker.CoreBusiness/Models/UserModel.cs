//-----------------------------------------------------------------------
// <copyright file="UserModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class UserModel
{

	[BsonId]
	[BsonElement("_id")]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	[BsonElement("object_identifier")]
	public string ObjectIdentifier { get; set; } = string.Empty;

	[BsonElement("first_name")]
	[BsonRepresentation(BsonType.String)]
	public string FirstName { get; set; } = string.Empty;

	[BsonElement("last_name")]
	[BsonRepresentation(BsonType.String)]
	public string LastName { get; set; } = string.Empty;

	[BsonElement("display_name")]
	[BsonRepresentation(BsonType.String)]
	public string DisplayName { get; set; } = string.Empty;

	[BsonElement("email_address")]
	[BsonRepresentation(BsonType.String)]
	public string EmailAddress { get; set; } = string.Empty;

	[BsonElement("archive")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archive { get; set; } = false;

}