//-----------------------------------------------------------------------
// <copyright file="ICategoryService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface ICategoryService
{
	Task<CategoryModel> GetCategory(string categoryId);

	Task<List<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(CategoryModel category);
}