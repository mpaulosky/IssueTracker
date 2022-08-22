//-----------------------------------------------------------------------
// <copyright file="ICategoryService.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.Library.Contracts;

public interface ICategoryService
{
	Task<CategoryModel> GetCategory(string id);

	Task<List<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(CategoryModel category);
}