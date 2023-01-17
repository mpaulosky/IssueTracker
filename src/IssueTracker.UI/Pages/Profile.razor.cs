//-----------------------------------------------------------------------
// <copyright file="Profile.razor.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022.2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///		Profile page class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
[UsedImplicitly]
public partial class Profile
{
	private List<IssueModel> _approved;
	private List<IssueModel> _archived;
	private List<CommentModel> _comments;
	private List<IssueModel> _issues;

	private UserModel _loggedInUser;
	private List<IssueModel> _pending;
	private List<IssueModel> _rejected;

	/// <summary>
	///		OnInitializedAsync event
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await Guard.Against.Null(AuthProvider.GetUserFromAuth(UserService),
			"AuthProvider.GetUserFromAuth(UserService) != null");

		_comments = await CommentService.GetCommentsByUser(_loggedInUser.Id);

		List<IssueModel> results = await IssueService.GetIssuesByUser(_loggedInUser.Id);

		if (results is not null)
		{
			_issues = results.OrderByDescending(s => s.DateCreated).ToList();

			_approved = _issues.Where(s => s.ApprovedForRelease && (s.Archived == false) & (s.Rejected == false))
				.ToList();

			_archived = _issues.Where(s => s.Archived && s.Rejected == false).ToList();

			_pending = _issues.Where(s => s.ApprovedForRelease == false && s.Rejected == false).ToList();

			_rejected = _issues.Where(s => s.Rejected).ToList();
		}
	}

	/// <summary>
	///		ClosePage method
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}