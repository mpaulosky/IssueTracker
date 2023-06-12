// Copyright (c) 2023. All rights reserved.
// File Name :     CommentModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class CommentModel
{
	/// <summary>
	///   Gets or sets the identifier.
	/// </summary>
	/// <value>
	///   The identifier.
	/// </value>
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the title.
	/// </summary>
	/// <value>
	///   The title.
	/// </value>
	[BsonElement("comment_title")]
	[BsonRepresentation(BsonType.String)]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the description.
	/// </summary>
	/// <value>
	///   The description.
	/// </value>
	[BsonElement("comment_description")]
	[BsonRepresentation(BsonType.String)]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the date created.
	/// </summary>
	/// <value>
	///   The date created.
	/// </value>
	[BsonElement("date_created")]
	[BsonRepresentation(BsonType.DateTime)]
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;

	/// <summary>
	///   Gets or sets the source.
	/// </summary>
	/// <value>
	///   The source.
	/// </value>
	public BasicCommentOnSourceModel? CommentOnSource { get; set; }

	/// <summary>
	///   Gets or sets the author.
	/// </summary>
	/// <value>
	///   The author.
	/// </value>
	public BasicUserModel Author { get; set; } = new();

	/// <summary>
	///   Gets or sets the user votes.
	/// </summary>
	/// <value>
	///   The user votes.
	/// </value>
	public HashSet<string> UserVotes { get; set; } = new();

	/// <summary>
	///   Gets or sets a value indicating whether this <see cref="CommentModel" /> is archived.
	/// </summary>
	/// <value>
	///   <c>true</c> if archived; otherwise, <c>false</c>.
	/// </value>
	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; }

	/// <summary>
	///   Gets or sets who archived the record.
	/// </summary>
	/// <value>
	///   Who archived the record.
	/// </value>
	public BasicUserModel ArchivedBy { get; set; } = new();
}