// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Category;

/// <summary>
///   CategoryService class
/// </summary>
public class CategoryService(ICategoryRepository repository, IMemoryCache cache) : ICategoryService
{
	private const string CacheName = "CategoryData";

	/// <summary>
	///   CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task CreateCategory(CategoryModel category)
	{
		ArgumentNullException.ThrowIfNull(category);

		await repository.CreateAsync(category);

		cache.Remove(CacheName);
	}

	/// <summary>
	///   ArchiveCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task ArchiveCategory(CategoryModel category)
	{
		ArgumentNullException.ThrowIfNull(category);

		await repository.ArchiveAsync(category);

		cache.Remove(CacheName);
	}

	/// <summary>
	///   GetCategory method
	/// </summary>
	/// <param name="categoryId">string</param>
	/// <returns>Task of CategoryModel</returns>
	/// <exception cref="ArgumentException">ThrowIfNullOrEmpty(categoryId)</exception>
	public async Task<CategoryModel> GetCategory(string? categoryId)
	{
		ArgumentException.ThrowIfNullOrEmpty(categoryId);

		CategoryModel result = await repository.GetAsync(categoryId);

		return result;
	}

	/// <summary>
	///   GetCategories method
	/// </summary>
	/// <returns>Task of List CategoryModel</returns>
	public async Task<List<CategoryModel>> GetCategories()
	{
		List<CategoryModel>? output = cache.Get<List<CategoryModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		IEnumerable<CategoryModel> results = await repository.GetAllAsync();

		output = results.ToList();

		cache.Set(CacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	/// <summary>
	///   UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task UpdateCategory(CategoryModel category)
	{
		ArgumentNullException.ThrowIfNull(category);

		await repository.UpdateAsync(category.Id, category);

		cache.Remove(CacheName);
	}
}