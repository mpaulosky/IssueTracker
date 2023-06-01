//-----------------------------------------------------------------------
// <copyright file="CategoryRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
// Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.DataAccess;

/// <summary>
///   CategoryRepository class
/// </summary>
public class CategoryRepository : ICategoryRepository
{
	private readonly IMongoCollection<CategoryModel> _collection;

	/// <summary>
	///   CategoryRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CategoryRepository(IMongoDbContextFactory context)
	{
		ArgumentNullException.ThrowIfNull(context);

		var collectionName = GetCollectionName(nameof(CategoryModel));

		_collection = context.GetCollection<CategoryModel>(collectionName);
	}

	/// <summary>
	///   Archive Category method
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	public async Task ArchiveAsync(CategoryModel category)
	{
		// Archive the category
		category.Archived = true;

		await UpdateAsync(category.Id, category);
	}

	/// <summary>
	///   Create Category method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task CreateAsync(CategoryModel category)
	{
		await _collection.InsertOneAsync(category);
	}

	/// <summary>
	///   Get Category method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <returns>Task of CategoryModel</returns>
	public async Task<CategoryModel> GetAsync(string? itemId)
	{
		var objectId = new ObjectId(itemId);

		var filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		var result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;
	}

	/// <summary>
	///   Get Categories method
	/// </summary>
	/// <returns>Task of IEnumerable CategoryModel</returns>
	public async Task<IEnumerable<CategoryModel>> GetAllAsync()
	{
		var filter = Builders<CategoryModel>.Filter.Empty;

		var result = (await _collection.FindAsync(filter)).ToList();

		return result;
	}

	/// <summary>
	///   Update Category method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="category">CategoryModel</param>
	public async Task UpdateAsync(string? itemId, CategoryModel category)
	{
		var objectId = new ObjectId(itemId);

		var filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, category);
	}
}
