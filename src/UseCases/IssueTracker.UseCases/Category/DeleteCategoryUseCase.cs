//-----------------------------------------------------------------------
// <copyright File="DeleteCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Category;

public class DeleteCategoryUseCase : IDeleteCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public DeleteCategoryUseCase(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task ExecuteAsync(CategoryModel category)
	{

		if (category == null) return;

		// Deactivate Category
		category.Archive = false;

		await _categoryRepository.UpdateCategoryAsync(category);

	}

}
