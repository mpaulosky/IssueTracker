//-----------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Contracts;

public interface ICategoryRepository
{

	Task ArchiveCategory(CategoryModel category);

	Task CreateCategory(CategoryModel category);

	Task<CategoryModel> GetCategory(string itemId);

	Task<IEnumerable<CategoryModel>> GetCategories();

	Task UpdateCategory(string itemId, CategoryModel category);

}