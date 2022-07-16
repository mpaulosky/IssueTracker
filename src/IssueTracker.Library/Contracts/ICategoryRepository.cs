namespace IssueTracker.Library.Contracts;

public interface ICategoryRepository
{
	Task<CategoryModel> GetCategory(string id);

	Task<IEnumerable<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(string id, CategoryModel category);
}