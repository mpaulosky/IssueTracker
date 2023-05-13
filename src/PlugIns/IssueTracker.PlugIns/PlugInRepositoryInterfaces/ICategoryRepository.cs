//-----------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.PlugInRepositoryInterfaces;

public interface ICategoryRepository
{

	Task ArchiveAsync(CategoryModel category);

	Task CreateAsync(CategoryModel category);

	Task<CategoryModel> GetAsync(string itemId);

	Task<IEnumerable<CategoryModel>> GetAllAsync();

	Task UpdateAsync(string itemId, CategoryModel category);

}
