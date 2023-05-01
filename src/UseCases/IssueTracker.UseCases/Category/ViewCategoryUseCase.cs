//-----------------------------------------------------------------------
// <copyright File="ViewCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class ViewCategoryUseCase : IViewCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public ViewCategoryUseCase(ICategoryRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;

	}

	public async Task<CategoryModel?> ExecuteAsync(string categoryId)
	{

		Guard.Against.NullOrWhiteSpace(categoryId, nameof(categoryId));

		return await _categoryRepository.GetAsync(categoryId);

	}

}
