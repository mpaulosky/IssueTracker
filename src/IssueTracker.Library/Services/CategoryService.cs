using Microsoft.Extensions.Caching.Memory;

namespace IssueTracker.Library.Services;

public class CategoryService : ICategoryService
{
	private const string _cacheName = "CategoryData";
	private readonly IMemoryCache _cache;
	private readonly ICategoryRepository _repository;

	public CategoryService(ICategoryRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	public async Task<CategoryModel> GetCategory(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var result = await _repository.GetCategory(id);

		return result;
	}

	public async Task<List<CategoryModel>> GetCategories()
	{
		var output = _cache.Get<List<CategoryModel>>(_cacheName);
		if (output is not null)
		{
			return output;
		}

		var results = await _repository.GetCategories();
		output = results.ToList();

		_cache.Set(_cacheName, output, TimeSpan.FromDays(1));

		return output;
	}

	public Task CreateCategory(CategoryModel category)
	{
		if (category == null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		return _repository.CreateCategory(category);
	}

	public Task UpdateCategory(CategoryModel category)
	{
		if (category == null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		return _repository.UpdateCategory(category.Id, category);
	}
}