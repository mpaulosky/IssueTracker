// Copyright (c) 2023. All rights reserved.
// File Name :     ICategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

public interface ICategoryRepository
{
	Task ArchiveAsync(CategoryModel category);

	Task CreateAsync(CategoryModel category);

	Task<CategoryModel> GetAsync(string? itemId);

	Task<IEnumerable<CategoryModel>> GetAllAsync();

	Task UpdateAsync(string? itemId, CategoryModel category);
}
