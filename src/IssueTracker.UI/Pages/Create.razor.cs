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
[UsedImplicitly]
public partial class Create
{
	private List<CategoryModel>? _categories;
	private List<StatusModel>? _statuses;
	private CreateIssueDto _issue = new();
	private UserModel? _loggedInUser;

	/// <summary>
	///		OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await Guard.Against.Null(AuthProvider.GetUserFromAuth(UserService),
			"AuthProvider.GetUserFromAuth(UserService) != null");
		_categories = await CategoryService.GetCategories();
		_statuses = await StatusService.GetStatuses();
	}

	/// <summary>
	///		CreateIssue method
	/// </summary>
	private async Task CreateIssue()
	{
		CategoryModel? category = _categories!.FirstOrDefault(c => c.Id == _issue.CategoryId);
		StatusModel? status = _statuses!.FirstOrDefault(c => c.StatusName == "Watching");
		IssueModel? s = new()
		{
			Title = _issue.Issue,
			Description = _issue.Description,
			Author = new BasicUserModel(_loggedInUser!),
			Category = new BasicCategoryModel(category!.CategoryName, category!.CategoryDescription),
			IssueStatus = new BasicStatusModel(status!.StatusName, status!.StatusDescription)
		};

		await IssueService.CreateIssue(s);

		_issue = new CreateIssueDto();
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