//-----------------------------------------------------------------------
// <copyright>
//	File:		EditCategoryUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class EditCategoryUseCase : IEditCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public EditCategoryUseCase(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task ExecuteAsync(CategoryModel? category)
	{

		if (category == null) return;

		await _categoryRepository.UpdateCategoryAsync(category);

	}

}
