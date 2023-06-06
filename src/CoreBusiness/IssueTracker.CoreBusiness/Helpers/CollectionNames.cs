//-----------------------------------------------------------------------// <copyright>//	File:		CollectionNames.cs//	Company:mpaulosky//	Author:	Matthew Paulosky//	Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.CoreBusiness.Helpers;/// <summary>
///   CollectionNames class
/// </summary>
public static class CollectionNames{	/// <summary>
	///   GetCollectionName method
	/// </summary>
	/// <param name="entityName">string</param>
	/// <returns>string collection name</returns>
	public static string GetCollectionName(string entityName)	{		return entityName switch		{			"CategoryModel" => "categories",			"CommentModel" => "comments",			"IssueModel" => "issues",			"SolutionModel" => "solutions",			"StatusModel" => "statuses",			"UserModel" => "users",			_ => ""		};	}}
