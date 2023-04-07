namespace IssueTracker.UseCases.Category.Interfaces;

public interface IViewCategoryByIdUseCase
{
	Task<CategoryModel> ExecuteAsync(string categoryId);
}
