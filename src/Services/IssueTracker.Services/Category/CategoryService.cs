﻿//-----------------------------------------------------------------------// <copyright file="CategoryService.cs" company="mpaulosky">//		Author:  Matthew Paulosky//		Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.Services.Category;/// <summary>
///   CategoryService class
/// </summary>
public class CategoryService : ICategoryService{	private const string CacheName = "CategoryData";	private readonly IMemoryCache _cache;	private readonly ICategoryRepository _repository;	/// <summary>
	///   CategoryService constructor
	/// </summary>
	/// <param name="repository">ICategoryRepository</param>
	/// <param name="cache">IMemoryCache</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CategoryService(ICategoryRepository repository, IMemoryCache cache)
	{
		ArgumentNullException.ThrowIfNull(repository);
		ArgumentNullException.ThrowIfNull(cache);

		_repository = repository;
		_cache = cache;
	}	/// <summary>
	///   CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateCategory(CategoryModel category)
	{
		ArgumentNullException.ThrowIfNull(category);

		_cache.Remove(CacheName);

		return _repository.CreateAsync(category);
	}	/// <summary>
	///   DeleteCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task DeleteCategory(CategoryModel category)
	{
		ArgumentNullException.ThrowIfNull(category);

		_cache.Remove(CacheName);

		return _repository.ArchiveAsync(category);
	}	/// <summary>
	///   GetCategory method
	/// </summary>
	/// <param name="categoryId">string</param>
	/// <returns>Task of CategoryModel</returns>
	/// <exception cref="ArgumentException">ThrowIfNullOrEmpty(categoryId)</exception>
	public async Task<CategoryModel> GetCategory(string? categoryId)
	{
		ArgumentException.ThrowIfNullOrEmpty(categoryId);

		var result = await _repository.GetAsync(categoryId);

		return result;
	}	/// <summary>
	///   GetCategories method
	/// </summary>
	/// <returns>Task of List CategoryModel</returns>
	public async Task<List<CategoryModel>> GetCategories()
	{
		var output = _cache.Get<List<CategoryModel>>(CacheName);

		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetAllAsync();

		output = results.ToList();

		_cache.Set(CacheName, output, TimeSpan.FromDays(1));

		return output;
	}	/// <summary>
	///   UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateCategory(CategoryModel category)	{		ArgumentNullException.ThrowIfNull(category);		_cache.Remove(CacheName);		return _repository.UpdateAsync(category.Id, category);	}}
