//-----------------------------------------------------------------------
// <copyright File="ArchiveCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Ardalis.GuardClauses;

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

		Guard.Against.Null(category, nameof(category));

		// Archive the category
		category.Archived = true;

		await _categoryRepository.UpdateAsync(category);

	}

}
