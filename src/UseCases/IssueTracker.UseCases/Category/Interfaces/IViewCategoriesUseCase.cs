namespace IssueTracker.UseCases.Category.Interfaces;

public interface IViewCategoriesUseCase
{

	Task<IEnumerable<CategoryModel>?> ExecuteAsync(bool includeArchived = false);

}
