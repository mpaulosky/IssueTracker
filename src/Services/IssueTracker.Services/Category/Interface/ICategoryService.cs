// Copyright (c) 2023. All rights reserved.
// File Name :     ICategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services

namespace IssueTracker.Services.Category.Interface;

public interface ICategoryService
{
	Task<CategoryModel> GetCategory(string? categoryId);

	Task<List<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(CategoryModel category);

	Task DeleteCategory(CategoryModel category);
}
