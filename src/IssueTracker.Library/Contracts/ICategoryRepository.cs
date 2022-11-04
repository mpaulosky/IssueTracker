//-----------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface ICategoryRepository
{

	Task<CategoryModel> GetCategory(string itemId);

	Task<IEnumerable<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(string itemId, CategoryModel category);

	Task DeleteCategory(CategoryModel category);

}