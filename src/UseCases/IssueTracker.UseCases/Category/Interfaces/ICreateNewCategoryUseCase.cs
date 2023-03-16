//-----------------------------------------------------------------------
// <copyright File="ICreateNewCategoryUseCase"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UseCases.Category.Interfaces;

public interface ICreateNewCategoryUseCase
{
	Task ExecuteAsync(CategoryModel category);
}