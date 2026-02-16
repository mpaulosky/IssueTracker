// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     ICategoryService.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.Services
// =============================================

namespace IssueTracker.Services.Category.Interface;

/// <summary>
/// Provides operations for managing issue categories in the system.
/// </summary>
public interface ICategoryService
{
	/// <summary>
	/// Archives an existing category, marking it as inactive.
	/// </summary>
	/// <param name="category">The category to archive.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="category"/> is null.</exception>
	Task ArchiveCategory(CategoryModel category);

	/// <summary>
	/// Creates a new category in the system.
	/// </summary>
	/// <param name="category">The category to create.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="category"/> is null.</exception>
	Task CreateCategory(CategoryModel category);

	/// <summary>
	/// Retrieves a specific category by its unique identifier.
	/// </summary>
	/// <param name="categoryId">The unique identifier of the category.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the requested <see cref="CategoryModel"/>.
	/// </returns>
	/// <exception cref="ArgumentException">Thrown when <paramref name="categoryId"/> is null or empty.</exception>
	Task<CategoryModel> GetCategory(string? categoryId);

	/// <summary>
	/// Retrieves all categories from the system with caching support.
	/// </summary>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a list of all <see cref="CategoryModel"/> instances.
	/// </returns>
	/// <remarks>
	/// This method uses in-memory caching with a 1-day expiration to improve performance.
	/// </remarks>
	Task<List<CategoryModel>> GetCategories();

	/// <summary>
	/// Updates an existing category with new information.
	/// </summary>
	/// <param name="category">The category with updated information.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="category"/> is null.</exception>
	Task UpdateCategory(CategoryModel category);
}
