namespace IssueTracker.Library.Services;

/// <summary>
/// CategoryService class
/// </summary>
public class CategoryService : ICategoryService
{
	private const string _cacheName = "CategoryData";
	private readonly IMemoryCache _cache;
	private readonly ICategoryRepository _repository;

	/// <summary>
	/// CategoryService constructor
	/// </summary>
	/// <param name="repository">ICategoryRepository</param>
	/// <param name="cache">IMemoryCache</param>
	public CategoryService(ICategoryRepository repository, IMemoryCache cache)
	{
		_repository = repository;
		_cache = cache;
	}

	/// <summary>
	/// GetCategory method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of CategoryModel</returns>
	/// <exception cref="ArgumentException"></exception>
	public async Task<CategoryModel> GetCategory(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
		}

		var result = await _repository.GetCategory(id);

		return result;
	}

	/// <summary>
	/// GetCategories method
	/// </summary>
	/// <returns>Task of List CategoryModel</returns>
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

	/// <summary>
	/// CreateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task CreateCategory(CategoryModel category)
	{
		if (category == null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		return _repository.CreateCategory(category);
	}

	/// <summary>
	/// UpdateCategory method
	/// </summary>
	/// <param name="category">CategoryModel</param>
	/// <returns>Task</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public Task UpdateCategory(CategoryModel category)
	{
		if (category == null)
		{
			throw new ArgumentNullException(nameof(category));
		}

		return _repository.UpdateCategory(category.Id, category);
	}
}