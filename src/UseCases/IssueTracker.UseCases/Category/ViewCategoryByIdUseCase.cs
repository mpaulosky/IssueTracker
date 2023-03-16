//-----------------------------------------------------------------------
// <copyright File="ViewCategoryByIdUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class ViewCategoryByIdUseCase : IViewCategoryByIdUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public ViewCategoryByIdUseCase(ICategoryRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;

	}

	public async Task<CategoryModel> ExecuteAsync(string categoryId)
	{

		if (string.IsNullOrWhiteSpace(categoryId)) return new();

		return await _categoryRepository.ViewCategoryByIdAsync(categoryId);

	}

}
