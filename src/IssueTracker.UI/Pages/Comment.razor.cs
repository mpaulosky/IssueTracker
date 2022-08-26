//-----------------------------------------------------------------------
// <copyright file="Comment.razor.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) .2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///   Comment page class.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
public partial class Comment
{
	private CreateCommentModel _comment = new();

	private IssueModel _issue;

	private UserModel _loggedInUser;

	[Parameter] public string Id { get; set; }

	/// <summary>
	///   OnInitializedAsync event.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_issue = await IssueService.GetIssue(Id);
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
	}

	/// <summary>
	///   CreateComment method.
	/// </summary>
	private async Task CreateComment()
	{
		CommentModel comment = new()
		{
			Issue = new BasicIssueModel(_issue), Author = new BasicUserModel(_loggedInUser), Comment = _comment.Comment
		};

		await CommentService.CreateComment(comment);

		_comment = new CreateCommentModel();
		ClosePage();
	}

	/// <summary>
	///   ClosePage method.
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}