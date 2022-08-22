//-----------------------------------------------------------------------
// <copyright file="CollectionNames.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Helpers;

public static class CollectionNames
{
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