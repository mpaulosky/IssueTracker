//-----------------------------------------------------------------------
// <copyright File="BasicCategoryModel.cs"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class BasicCategoryModel
{
	public BasicCategoryModel()
	{
	}

	public BasicCategoryModel(CategoryModel category)
	{
		CategoryName = category?.CategoryName;
		CategoryDescription = category?.CategoryDescription;
	}

	public BasicCategoryModel(string categoryName, string categoryDescription)
	{
		CategoryName = categoryName;
		CategoryDescription = categoryDescription;
	}

	public string CategoryName { get; init; }

	public string CategoryDescription { get; init; }
}