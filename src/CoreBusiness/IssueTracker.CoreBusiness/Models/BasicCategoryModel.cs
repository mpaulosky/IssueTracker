//-----------------------------------------------------------------------// <copyright>//	File:		BasicCategoryModel.cs//	Company:mpaulosky//	Author:	Matthew Paulosky//	Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   Basic Category Model class
/// </summary>
[Serializable]public class BasicCategoryModel{
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicCategoryModel" /> class.
	/// </summary>
	public BasicCategoryModel()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicCategoryModel" /> class.
	/// </summary>
	/// <param name="category">The category.</param>
	public BasicCategoryModel(CategoryModel category)
	{
		CategoryName = category.CategoryName;
		CategoryDescription = category.CategoryDescription;
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicCategoryModel" /> class.
	/// </summary>
	/// <param name="categoryName">Name of the category.</param>
	/// <param name="categoryDescription">The category description.</param>
	public BasicCategoryModel(string categoryName, string categoryDescription) : this()
	{
		CategoryName = categoryName;
		CategoryDescription = categoryDescription;
	}

	/// <summary>
	///   Gets the name of the category.
	/// </summary>
	/// <value>
	///   The name of the category.
	/// </value>
	public string CategoryName { get; init; } = string.Empty;

	/// <summary>
	///   Gets the category description.
	/// </summary>
	/// <value>
	///   The category description.
	/// </value>
	public string CategoryDescription { get; init; } = string.Empty;}
