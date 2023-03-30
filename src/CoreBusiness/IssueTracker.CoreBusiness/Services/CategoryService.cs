//-----------------------------------------------------------------------
// <copyright file="CategoryService.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.CoreBusiness.Contracts;
using IssueTracker.CoreBusiness.Services.Interfaces;

using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.CoreBusiness.Services;

/// <summary>
///		CategoryService class
/// </summary>
public class CategoryService : ICategoryService
{

	private const string _cacheName = "CategoryData";
	private readonly IMemoryCache _cache;
	private readonly ICategoryRepository _repository;

	/// <summary>
	///		CategoryService constructor
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
	///		CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateCategory(CategoryModel category)
	{

		Guard.Against.Null(category, nameof(category));

		_cache.Remove(_cacheName);

		return _repository.CreateCategory(category);

	}

	/// <summary>
	///  DeleteCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task DeleteCategory(CategoryModel category)
	{

		Guard.Against.Null(category, nameof(category));

		_cache.Remove(_cacheName);

		return _repository.ArchiveCategory(category);

	}

	/// <summary>
	///		GetCategory method
	/// </summary>
	/// <param name="categoryId">string</param>
	/// <returns>Task of CategoryModel</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task<CategoryModel> GetCategory(string categoryId)
	{

		Guard.Against.NullOrWhiteSpace(categoryId, nameof(categoryId));

		CategoryModel result = await _repository.GetCategory(categoryId);

		return result;

	}

	/// <summary>
	///		GetCategories method
	/// </summary>
	/// <returns>Task of List CategoryModel</returns>
	public async Task<List<CategoryModel>> GetCategories()
	{

		List<CategoryModel>? output = _cache.Get<List<CategoryModel>>(_cacheName);

		if (output is not null) return output;

		IEnumerable<CategoryModel> results = await _repository.GetCategories();

		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));

		return output;

	}

	/// <summary>
	///		UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateCategory(CategoryModel category)
	{

		Guard.Against.Null(category, nameof(category));

		_cache.Remove(_cacheName);

		return _repository.UpdateCategory(category.Id, category);

	}

}