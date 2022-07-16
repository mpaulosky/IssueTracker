using static IssueTracker.Library.Helpers.CollectionNames;

namespace IssueTracker.Library.DataAccess;

public class CategoryRepository : ICategoryRepository
{
	private readonly IMongoCollection<CategoryModel> _collection;

	public CategoryRepository(IMongoDbContext context)
	{
		_collection = context.GetCollection<CategoryModel>(GetCollectionName(nameof(CategoryModel)));
	}

	public async Task<CategoryModel> GetCategory(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<CategoryModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter);

		return result.FirstOrDefault();
	}

	public async Task<IEnumerable<CategoryModel>> GetCategories()
	{
		var all = await _collection.FindAsync(Builders<CategoryModel>.Filter.Empty);

		return await all.ToListAsync();
	}

	public async Task CreateCategory(CategoryModel category)
	{
		await _collection.InsertOneAsync(category);
	}

	public async Task UpdateCategory(string id, CategoryModel category)
	{
		await _collection.ReplaceOneAsync(Builders<CategoryModel>.Filter.Eq("_id", id), category);
	}
}