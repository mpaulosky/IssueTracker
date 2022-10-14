//-----------------------------------------------------------------------
// <copyright file="UserModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class UserModel
{
	[BsonId]
	[BsonElement("_id")]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; }

	[BsonElement("object_identifier")] 
	public string? ObjectIdentifier { get; set; }

	[BsonElement("first_name")]
	[BsonRepresentation(BsonType.String)]
	public string? FirstName { get; set; }

	[BsonElement("last_name")]
	[BsonRepresentation(BsonType.String)]
	public string? LastName { get; set; }

	[BsonElement("display_name")]
	[BsonRepresentation(BsonType.String)]
	public string? DisplayName { get; set; }

	[BsonElement("email_address")]
	[BsonRepresentation(BsonType.String)]
	public string? EmailAddress { get; set; }
}