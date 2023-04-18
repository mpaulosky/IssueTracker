//-----------------------------------------------------------------------
// <copyright File="ViewCategoriesUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class ViewCategoriesUseCase : IViewCategoriesUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public ViewCategoriesUseCase(ICategoryRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;

	}

	public async Task<IEnumerable<CategoryModel>> ExecuteAsync()
	{

		return await _categoryRepository.GetCategoriesAsync();

	}

}
