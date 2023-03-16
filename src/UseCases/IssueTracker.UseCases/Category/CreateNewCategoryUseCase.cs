//-----------------------------------------------------------------------
// <copyright File="CreateNewCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UseCases.PlugInRepositoryInterfaces;

namespace IssueTracker.UseCases.Category;

public class CreateNewCategoryUseCase : ICreateNewCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public CreateNewCategoryUseCase(ICategoryRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;

	}

	public async Task ExecuteAsync(CategoryModel category)
	{

		if (category == null) return;

		await _categoryRepository.CreateNewCategoryAsync(category);

	}

}
