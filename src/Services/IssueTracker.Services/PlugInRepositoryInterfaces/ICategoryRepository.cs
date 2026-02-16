// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICategoryRepository.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.PlugInRepositoryInterfaces;

/// <summary>
/// Provides data access operations for category entities in the data store.
/// </summary>
public interface ICategoryRepository
{
	/// <summary>
	/// Archives a category by marking it as inactive in the data store.
	/// </summary>
	/// <param name="category">The category to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="category"/> is null.</exception>
	Task ArchiveAsync(CategoryModel category);

	/// <summary>
	/// Creates a new category in the data store.
	/// </summary>
	/// <param name="category">The category to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="category"/> is null.</exception>
	Task CreateAsync(CategoryModel category);

	/// <summary>
	/// Retrieves a specific category from the data store by its unique identifier.
	/// </summary>
	/// <param name="itemId">The unique identifier of the category.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="CategoryModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	Task<CategoryModel> GetAsync(string? itemId);

	/// <summary>
	/// Retrieves all categories from the data store.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains an enumerable collection of all <see cref="CategoryModel"/> instances.
	/// </returns>
	Task<IEnumerable<CategoryModel>> GetAllAsync();

	/// <summary>
	/// Updates an existing category in the data store.
	/// </summary>
	/// <param name="itemId">The unique identifier of the category to update.</param>
	/// <param name="category">The updated category data.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="itemId"/> is null or empty.</exception>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="category"/> is null.</exception>
	Task UpdateAsync(string? itemId, CategoryModel category);
}
