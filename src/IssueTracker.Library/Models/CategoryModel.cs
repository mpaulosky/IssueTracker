﻿//-----------------------------------------------------------------------
// <copyright file="CategoryModel.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class CategoryModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	[BsonElement("category_name")]
	[BsonRepresentation(BsonType.String)]
	public string CategoryName { get; set; }

	[BsonElement("category-description")]
	[BsonRepresentation(BsonType.String)]
	public string CategoryDescription { get; set; }
}