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
	/// ArchiveCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task ArchiveAsync(CategoryModel category)
	{

		var objectId = new ObjectId(category.Id);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, category);

	}


	/// <summary>
	///		CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task CreateAsync(CategoryModel category)
	{

		await _collection.InsertOneAsync(category);

	}

	/// <summary>
	///		GetCategory method
	/// </summary>
	/// <param name="categoryId">string</param>
	/// <returns>Task of CategoryModel</returns>
	public async Task<CategoryModel?> GetAsync(string categoryId)
	{

		return (await _collection
			.FindAsync(s => s.Id == categoryId && s.Archived == false))
			.FirstOrDefault();

	}


	/// <summary>
	///		GetCategories method
	/// </summary>
	/// <param name="includeArchived">bool</param>
	/// <returns>Task of IEnumerable CategoryModel</returns>
	public async Task<IEnumerable<CategoryModel>?> GetAllAsync(bool includeArchived = false)
	{

		if (includeArchived)
		{

			var filter = Builders<CategoryModel>.Filter.Empty;
			return (await _collection
					.FindAsync(filter))
					.ToList();

		}
		else
		{

			return (await _collection
					.FindAsync(x => x.Archived == includeArchived))
					.ToList();

		}

	}

	/// <summary>
	///		UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	public async Task UpdateAsync(CategoryModel category)
	{

		var objectId = new ObjectId(category.Id);

		FilterDefinition<CategoryModel> filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		await _collection.ReplaceOneAsync(filter, category);

	}

}
