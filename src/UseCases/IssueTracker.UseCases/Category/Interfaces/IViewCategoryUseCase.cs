namespace IssueTracker.UseCases.Category.Interfaces;

public interface IViewCategoryUseCase
{
	Task<CategoryModel?> ExecuteAsync(string? categoryId);
}
