//-----------------------------------------------------------------------
// <copyright file="CategoryRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

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
	public CategoryRepository(IMongoDbContext context)
	{
		_collection = context?.GetCollection<CategoryModel>(GetCollectionName(nameof(CategoryModel)));
	}

	/// <summary>
	///   GetCategory method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of CategoryModel</returns>
	public async Task<CategoryModel> GetCategory(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter).ConfigureAwait(true);

		return result.FirstOrDefault();
	}

	/// <summary>
	///   GetCategories method
	/// </summary>
	/// <returns>Task of IEnumerable CategoryModel</returns>
	public async Task<IEnumerable<CategoryModel>> GetCategories()
	{
		var all = await _collection.FindAsync(Builders<CategoryModel>.Filter.Empty).ConfigureAwait(true);

		return await all.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task CreateCategory(CategoryModel category)
	{
		await _collection.InsertOneAsync(category).ConfigureAwait(true);
	}

	/// <summary>
	///   UpdateCategory method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="category">CategoryModel</param>
	public async Task UpdateCategory(string id, CategoryModel category)
	{
		await _collection.ReplaceOneAsync(Builders<CategoryModel>.Filter.Eq("_id", id),
			category).ConfigureAwait(true);
	}
}