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

	Task ArchiveAsync(CategoryModel category);

	Task CreateAsync(CategoryModel category);

	Task<CategoryModel?> GetAsync(string? categoryId);

	Task<IEnumerable<CategoryModel>?> GetAllAsync(bool includeArchived = false);

	Task UpdateAsync(CategoryModel category);

}
