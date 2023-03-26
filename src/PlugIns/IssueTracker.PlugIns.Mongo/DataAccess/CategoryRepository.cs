//-----------------------------------------------------------------------
// <copyright file="CategoryRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
// Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ICategoryRepository = IssueTracker.CoreBusiness.Contracts.ICategoryRepository;

namespace IssueTracker.Library.DataAccess;

/// <summary>
///		CategoryRepository class
/// </summary>
public class CategoryRepository : ICategoryRepository
{
	private readonly IMongoCollection<CategoryModel> _collection;

	/// <summary>
	///		CategoryRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CategoryRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var collectionName = GetCollectionName(nameof(CategoryModel));

		_collection = context.GetCollection<CategoryModel>(collectionName);

	}

	/// <summary>
	///		GetCategory method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of CategoryModel</returns>
	public async Task<CategoryModel> GetCategory(string itemId)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		CategoryModel result = (await _collection!.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetCategories method
	/// </summary>
	/// <returns>Task of IEnumerable CategoryModel</returns>
	public async Task<IEnumerable<CategoryModel>> GetCategories()
	{

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Empty;

		var result = (await _collection!.FindAsync(filter)).ToList();

		return result;

	}

	/// <summary>
	///		CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task CreateCategory(CategoryModel category)
	{

		await _collection!.InsertOneAsync(category);

	}

	/// <summary>
	///		UpdateCategory method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="category">CategoryModel</param>
	public async Task UpdateCategory(string itemId, CategoryModel category)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, category);

	}

	/// <summary>
	///   DeleteCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task DeleteCategory(CategoryModel category)
	{

		var objectId = new ObjectId(category.Id);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		await _collection.DeleteOneAsync(filter);

	}
}