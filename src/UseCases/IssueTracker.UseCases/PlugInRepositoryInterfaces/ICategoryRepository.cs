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

	Task<CategoryModel> GetCategoryByIdAsync(string categoryId);

	Task<IEnumerable<CategoryModel>> GetCategoriesAsync();

	Task UpdateCategoryAsync(CategoryModel category);

}
