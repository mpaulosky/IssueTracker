// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Comment.razor.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

using Microsoft.AspNetCore.Authorization;

namespace IssueTracker.UI.Pages;

/// <summary>
///   Comment page class.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
public partial class Comment
{
	private CreateCommentDto _comment = new();

	private IssueModel? _issue;

	private UserModel? _loggedInUser;

	private bool _notFound;

	[Parameter] public string? Id { get; set; }

	/// <summary>
	///   OnInitializedAsync event.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);

		AuthenticationState authState = await AuthProvider.GetAuthenticationStateAsync();
		bool isAdmin = (await AuthorizationService.AuthorizeAsync(authState.User, "Admin")).Succeeded;

		_issue = await IssueService.GetIssue(Id, _loggedInUser.Id, isAdmin);
		_notFound = _issue is null;
	}

	/// <summary>
	///   CreateComment method.
	/// </summary>
	private async Task CreateComment()
	{
		CommentModel comment = new()
		{
			Issue = new BasicIssueModel(_issue!),
			Author = new BasicUserModel(_loggedInUser!),
			Title = _comment.Title!,
			Description = _comment.Description!
		};

		await CommentService.CreateComment(comment);

		_comment = new CreateCommentDto();

		ClosePage();
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
	///   ClosePage method.
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}