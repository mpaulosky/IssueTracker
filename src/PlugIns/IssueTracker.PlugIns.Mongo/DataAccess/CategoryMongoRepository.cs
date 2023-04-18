//-----------------------------------------------------------------------
// <copyright>
//	File:		CategoryMongoRepository.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.DataAccess;

/// <summary>
///		CategoryRepository class
/// </summary>
public class CategoryMongoRepository : ICategoryRepository
{

	private readonly IMongoCollection<CategoryModel> _collection;

	/// <summary>
	///		CategoryRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public CategoryMongoRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var collectionName = GetCollectionName(nameof(CategoryModel));

		_collection = context.GetCollection<CategoryModel>(collectionName);

	}

	/// <summary>
	///		GetCategory method
	/// </summary>
	/// <param name="categoryId">string</param>
	/// <returns>Task of CategoryModel</returns>
	public async Task<CategoryModel> GetCategoryByIdAsync(string categoryId)
	{

		var objectId = new ObjectId(categoryId);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		CategoryModel result = (await _collection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetCategories method
	/// </summary>
	/// <returns>Task of IEnumerable CategoryModel</returns>
	public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
	{

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Empty;

		var result = (await _collection.FindAsync(filter)).ToList();

		return result;

	}

	/// <summary>
	///		CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task CreateCategoryAsync(CategoryModel category)
	{

		await _collection.InsertOneAsync(category);

	}

	/// <summary>
	///		UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task UpdateCategoryAsync(CategoryModel category)
	{

		var objectId = new ObjectId(category.Id);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, category);

	}

}
