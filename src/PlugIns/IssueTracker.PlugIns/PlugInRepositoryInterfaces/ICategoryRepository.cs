//-----------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.PlugInRepositoryInterfaces;

public interface ICategoryRepository
{

	Task ArchiveCategoryAsync(CategoryModel category);

	Task CreateCategoryAsync(CategoryModel category);

	Task<CategoryModel> GetCategoryAsync(string itemId);

	Task<IEnumerable<CategoryModel>> GetCategoriesAsync();

	Task UpdateCategoryAsync(string itemId, CategoryModel category);

}
