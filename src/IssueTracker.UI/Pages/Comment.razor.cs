//-----------------------------------------------------------------------
// <copyright file="Comment.razor.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022.2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///		Comment page class.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
public partial class Comment
{
	private CreateCommentDto _comment = new();

	private IssueModel? _issue;

	private UserModel? _loggedInUser;

	[Parameter] public string? Id { get; set; }

	/// <summary>
	///		OnInitializedAsync event.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{

		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);

		_issue = await IssueService.GetIssue(issueId: Id);

	}

	/// <summary>
	///		CreateComment method.
	/// </summary>
	private async Task CreateComment()
	{
		CommentModel? comment = new()
		{
			CommentOnSource = new BasicCommentOnSourceModel(_issue!),
			Author = new BasicUserModel(_loggedInUser!),
			Title = _comment.Title!,
			Description = _comment.Description!
		};

		await CommentService.CreateComment(comment);

		_comment = new CreateCommentDto();

		ClosePage();
	}

	/// <summary>
	///		ClosePage method.
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}