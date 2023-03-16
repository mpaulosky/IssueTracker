namespace IssueTracker.UseCases.Category;

public class EditCategoryUseCase : IEditCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public EditCategoryUseCase(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task ExecuteAsync(CategoryModel category)
	{

		if (category == null) return;

		await _categoryRepository.UpdateCategoryAsync(category);

	}

}
