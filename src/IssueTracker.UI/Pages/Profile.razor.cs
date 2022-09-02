//-----------------------------------------------------------------------
// <copyright file="Profile.razor.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) .2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///   Profile page class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
public partial class Profile
{
	private List<IssueModel> _approved;
	private List<IssueModel> _archived;
	private List<IssueModel> _pending;
	private List<IssueModel> _rejected;
	private List<CommentModel> _comments;
	private List<IssueModel> _issues;

	private UserModel _loggedInUser;

	/// <summary>
	///   OnInitializedAsync event
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await Guard.Against.Null(AuthProvider.GetUserFromAuth(UserService), "AuthProvider.GetUserFromAuth(UserService) != null");

		_comments = await CommentService.GetUsersComments(_loggedInUser.Id);

		var results = await IssueService.GetUsersIssues(_loggedInUser.Id);

		if (results is not null)
		{
			_issues = results.OrderByDescending(s => s.DateCreated).ToList();

			_approved = _issues.Where(s => s.ApprovedForRelease && s.Archived == false & s.Rejected == false).ToList();

			_archived = _issues.Where(s => s.Archived && s.Rejected == false).ToList();

			_pending = _issues.Where(s => s.ApprovedForRelease == false && s.Rejected == false).ToList();

			_rejected = _issues.Where(s => s.Rejected).ToList();
		}
	}

	/// <summary>
	///   GetIssueStatusCssClass method
	/// </summary>
	/// <param name="issue">string</param>
	/// <returns>string</returns>
	private string GetIssueStatusCssClass(IssueModel issue)
	{
		string output = issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-profile-status issue-profile-status-answered",
			"In Work" => "issue-profile-status issue-profile-status-inwork",
			"Watching" => "issue-profile-status issue-profile-status-watching",
			"Dismissed" => "issue-profile-status issue-profile-status-dismissed",
			_ => "issue-profile-status issue-profile-status-none"
		};
		return output;
	}

	/// <summary>
	///   ClosePage method
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}