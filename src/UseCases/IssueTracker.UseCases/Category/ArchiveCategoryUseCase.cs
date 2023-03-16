//-----------------------------------------------------------------------
// <copyright File="ArchiveCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class ArchiveCategoryUseCase : IArchiveCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public ArchiveCategoryUseCase(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task ExecuteAsync(CategoryModel category)
	{

		if (category == null) return;

		// Archive the category
		category.Archived = true;

		await _categoryRepository.UpdateCategoryAsync(category);

	}

}
