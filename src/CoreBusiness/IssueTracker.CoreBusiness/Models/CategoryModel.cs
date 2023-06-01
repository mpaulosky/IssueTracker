//-----------------------------------------------------------------------
// <copyright>
//	File:		CategoryModel.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   CategoryModel class
/// </summary>
[Serializable]
public class CategoryModel
{
	/// <summary>
	///   Gets or sets the identifier.
	/// </summary>
	/// <value>
	///   The identifier.
	/// </value>
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? Id { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the name of the category.
	/// </summary>
	/// <value>
	///   The name of the category.
	/// </value>
	[BsonElement("category_name")]
	[BsonRepresentation(BsonType.String)]
	public string CategoryName { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the category description.
	/// </summary>
	/// <value>
	///   The category description.
	/// </value>
	[BsonElement("category-description")]
	[BsonRepresentation(BsonType.String)]
	public string CategoryDescription { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets a value indicating whether this <see cref="CategoryModel" /> is archived.
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
