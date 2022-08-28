//-----------------------------------------------------------------------
// <copyright file="ICategoryRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface ICategoryRepository
{
	Task<CategoryModel> GetCategory(string categoryId);

	Task<IEnumerable<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(string id, CategoryModel category);
}