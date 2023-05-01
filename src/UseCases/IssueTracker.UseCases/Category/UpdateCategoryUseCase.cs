//-----------------------------------------------------------------------
// <copyright>
//	File:		UpdateCategoryUseCase.cs
//	Company:mpaulosky
//	Author:	Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class UpdateCategoryUseCase : IUpdateCategoryUseCase
{

	readonly ICategoryRepository _categoryRepository;

	public UpdateCategoryUseCase(ICategoryRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;

	}

	public async Task ExecuteAsync(CategoryModel category)
	{

		Guard.Against.Null(category, nameof(category));

		await _categoryRepository.UpdateAsync(category);

	}

}
