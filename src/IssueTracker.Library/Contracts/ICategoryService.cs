namespace IssueTracker.Library.Contracts;

public interface ICategoryService
{
	Task<CategoryModel> GetCategory(string id);

	Task<List<CategoryModel>> GetCategories();

	Task CreateCategory(CategoryModel category);

	Task UpdateCategory(CategoryModel category);
}