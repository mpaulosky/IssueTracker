//-----------------------------------------------------------------------
// <copyright File="BasicCategoryModel.cs"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class BasicCategoryModel
{

	public BasicCategoryModel()
	{
	}

	public BasicCategoryModel(CategoryModel category)
	{
		CategoryName = category.CategoryName;
		CategoryDescription = category.CategoryDescription;
	}

	public BasicCategoryModel(string categoryName, string categoryDescription) : this()
	{
		CategoryName = categoryName;
		CategoryDescription = categoryDescription;
	}

	public string CategoryName { get; init; } = string.Empty;

	public string CategoryDescription { get; init; } = string.Empty;

}