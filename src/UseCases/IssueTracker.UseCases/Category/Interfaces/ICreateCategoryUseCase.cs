//-----------------------------------------------------------------------
// <copyright File="ICreateCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category.Interfaces;

public interface ICreateCategoryUseCase
{
	Task ExecuteAsync(CategoryModel category);
}
