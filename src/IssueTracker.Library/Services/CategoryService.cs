﻿//-----------------------------------------------------------------------
// <copyright file="CategoryService.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Services;

/// <summary>
///   CategoryService class
/// </summary>
public class CategoryService : ICategoryService
{
	private const string _cacheName = "CategoryData";
	private readonly IMemoryCache _cache;
	private readonly ICategoryRepository _repository;

	/// <summary>
	///   CategoryService constructor
	/// </summary>
	/// <param name="repository">ICategoryRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CategoryService(ICategoryRepository repository, IMemoryCache cache)
	{
		_repository = Guard.Against.Null(repository, nameof(repository));
		_cache = Guard.Against.Null(cache, nameof(cache));
	}

	/// <summary>
	///   GetCategory method
	/// </summary>
	/// <param name="categoryId">string</param>
	/// <returns>Task of CategoryModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<CategoryModel> GetCategory(string categoryId)
	{
		Guard.Against.NullOrWhiteSpace(categoryId, nameof(categoryId));

		var result = await _repository.GetCategory(categoryId).ConfigureAwait(true);

		return result;
	}

	/// <summary>
	///   GetCategories method
	/// </summary>
	/// <returns>Task of List CategoryModel</returns>
	public async Task<List<CategoryModel>> GetCategories()
	{
		var output = _cache.Get<List<CategoryModel>>(_cacheName);

		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetCategories().ConfigureAwait(true);

		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	/// <summary>
	///   CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateCategory(CategoryModel category)
	{
		Guard.Against.Null(category, nameof(category));

		return _repository.CreateCategory(category);
	}

	/// <summary>
	///   UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateCategory(CategoryModel category)
	{
		Guard.Against.Null(category, nameof(category));

		return _repository.UpdateCategory(category.Id, category);
	}
}