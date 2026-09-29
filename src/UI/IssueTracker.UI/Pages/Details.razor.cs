// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Details.razor.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.UI.Pages;

/// <summary>
///   Details class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
[UsedImplicitly]
public partial class Details
{
	private List<CommentModel>? _comments = new();

	private IssueModel? _issue = new();

	private UserModel? _loggedInUser = new();

	private bool _notFound;

	[Parameter] public string? Id { get; set; }

	/// <summary>
	///   OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);

		ArgumentNullException.ThrowIfNull(Id);

		AuthenticationState authState = await AuthProvider.GetAuthenticationStateAsync();
		bool isAdmin = (await AuthorizationService.AuthorizeAsync(authState.User, "Admin")).Succeeded;

		_issue = await IssueService.GetIssue(Id, _loggedInUser.Id, isAdmin);

		if (_issue is null)
		{
			_notFound = true;
			return;
		}

		BasicIssueModel issue = new(_issue);
		_comments = await CommentService.GetCommentsByIssue(issue);
		await StatusService.GetStatuses();
	}

	/// <summary>
	///   OpenCommentForm method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	private void OpenCommentForm(IssueModel issue)
	{
		if (_loggedInUser is not null)
		{
			NavManager.NavigateTo($"/Comment/{issue.Id}");
		}
	}

	/// <summary>
	///   ClosePage method
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}