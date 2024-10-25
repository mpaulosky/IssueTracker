﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CollectionNames.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.Helpers;

/// <summary>
///   CollectionNames class
/// </summary>
public static class CollectionNames
{
	/// <summary>
	///   GetCollectionName method
	/// </summary>
	/// <param name="entityName">string</param>
	/// <returns>string collection name</returns>
	public static string GetCollectionName(string entityName)
	{
		return entityName switch
		{
			"CategoryModel" => "categories",
			"CommentModel" => "comments",
			"IssueModel" => "issues",
			"StatusModel" => "statuses",
			"UserModel" => "users",
			_ => ""
		};
	}
}