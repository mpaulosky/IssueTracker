//-----------------------------------------------------------------------
// <copyright File="CreateCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category;

public class CreateCategoryUseCase : ICreateCategoryUseCase
{

	private readonly ICategoryRepository _categoryRepository;

	public CreateCategoryUseCase(ICategoryRepository categoryRepository)
	{

		_categoryRepository = categoryRepository;

	}

	public async Task ExecuteAsync(CategoryModel? category)
	{

		ArgumentNullException.ThrowIfNull(category);

		await _categoryRepository.CreateAsync(category);

	}

}
