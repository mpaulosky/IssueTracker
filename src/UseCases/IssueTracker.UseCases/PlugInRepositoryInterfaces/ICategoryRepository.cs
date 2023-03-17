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
	Task CreateCategoryAsync(CategoryModel category);
	Task UpdateCategoryAsync(CategoryModel category);
	Task<IEnumerable<CategoryModel>> GetCategoriesAsync();
	Task<CategoryModel> GetCategoryByIdAsync(string categoryId);
}