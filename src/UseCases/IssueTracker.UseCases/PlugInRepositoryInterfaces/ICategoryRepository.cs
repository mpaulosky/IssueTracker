//-----------------------------------------------------------------------
// <copyright File="ICategoryRepository"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.PlugInRepositoryInterfaces;

public interface ICategoryRepository
{
	Task CreateNewCategoryAsync(CategoryModel category);
	Task UpdateCategoryAsync(CategoryModel category);
	Task<IEnumerable<CategoryModel>> ViewCategoriesAsync();
	Task<CategoryModel> ViewCategoryByIdAsync(string categoryId);
}