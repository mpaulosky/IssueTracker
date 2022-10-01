//-----------------------------------------------------------------------
// <copyright file="Create.razor.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022.2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///		Create class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
public partial class Create
{
	private List<CategoryModel> _categories;
	private CreateIssueModel _issue = new();
	private UserModel _loggedInUser;

	/// <summary>
	///		OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await Guard.Against.Null(AuthProvider.GetUserFromAuth(UserService),
			"AuthProvider.GetUserFromAuth(UserService) != null");
		_categories = await CategoryService.GetCategories();
	}

	/// <summary>
	///		CreateIssue method
	/// </summary>
	private async Task CreateIssue()
	{
		var category = _categories.FirstOrDefault(c => c.Id == _issue.CategoryId);
		IssueModel s = new()
		{
			IssueName = _issue.Issue,
			Description = _issue.Description,
			Author = new BasicUserModel(_loggedInUser),
			Category = new BasicCategoryModel(category.Id, category.CategoryDescription)
		};

		await IssueService.CreateIssue(s);

		_issue = new CreateIssueModel();
		ClosePage();
	}

	/// <summary>
	///		ClosePage method
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}