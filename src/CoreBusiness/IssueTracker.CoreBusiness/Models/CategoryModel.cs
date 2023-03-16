//-----------------------------------------------------------------------
// <copyright file="CategoryModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class CategoryModel
{

	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; } = string.Empty;

	[BsonElement("category_name")]
	[BsonRepresentation(BsonType.String)]
	public string CategoryName { get; set; } = string.Empty;

	[BsonElement("category-description")]
	[BsonRepresentation(BsonType.String)]
	public string CategoryDescription { get; set; } = string.Empty;

	[BsonElement("archived")]
	[BsonRepresentation(BsonType.Boolean)]
	public bool Archived { get; set; } = false;

}