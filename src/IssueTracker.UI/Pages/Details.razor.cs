//-----------------------------------------------------------------------
// <copyright file="Details.razor.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022.2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///		Details class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
public partial class Details
{
	private List<CommentModel> _comments;

	private IssueModel _issue;

	private UserModel _loggedInUser;
	private string _settingStatus = "";
	private List<StatusModel> _statuses;
	private string _urlText = "";
	[Parameter] public string Id { get; set; }

	/// <summary>
	///		OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await Guard.Against.Null(AuthProvider.GetUserFromAuth(UserService),
			"AuthProvider.GetUserFromAuth(UserService) != null");
		Guard.Against.NullOrWhiteSpace(Id, nameof(Id));

		_issue = await IssueService.GetIssue(Id);
		_comments = await CommentService.GetIssuesComments(Id);
		_statuses = await StatusService.GetStatuses();
	}

	/// <summary>
	///		CompleteSetStatus method
	/// </summary>
	private async Task CompleteSetStatus()
	{
		switch (_settingStatus)
		{
			case "answered":
				if (string.IsNullOrWhiteSpace(_urlText))
				{
					return;
				}

				_issue.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));
				_issue.OwnerNotes =
					"This Issue has a voted answer for it’s solution";
				break;
			case "in work":
				_issue.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));
				_issue.OwnerNotes =
					"There has been an suggested answer for this issue submitted.";
				break;
			case "watching":
				_issue.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));
				_issue.OwnerNotes =
					"An Issue was submitted requesting help.";
				break;
			case "dismissed":
				_issue.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));
				_issue.OwnerNotes =
					"Sometimes an Issue does not have a clear solution, this is one of those.";
				break;
			default:
				return;
		}

		_settingStatus = null;

		await IssueService.UpdateIssue(_issue);
	}

	/// <summary>
	///		GetStatusCssClass method
	/// </summary>
	/// <returns>string css class</returns>
	private string GetStatusCssClass()
	{
		if (_issue.IssueStatus is null)
		{
			return "issue-detail-status-none";
		}
		
		var output = _issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-detail-status-answered",
			"In Work" => "issue-detail-status-inwork",
			"Watching" => "issue-detail-status-watching",
			"Dismissed" => "issue-detail-status-dismissed",
			_ => "issue-detail-status-none"
		};

		return output;
	}

	/// <summary>
	///		VoteUp method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	private async Task VoteUp(CommentModel comment)
	{
		if (_loggedInUser is not null)
		{
			if (comment.Author.Id == _loggedInUser.Id)
			{
				// Can't vote on your own comments
				return;
			}

			if (comment.UserVotes.Add(_loggedInUser.Id) == false)
			{
				comment.UserVotes.Remove(_loggedInUser.Id);
			}

			await CommentService.UpVoteComment(comment.Id, _loggedInUser.Id);
		}
	}

	/// <summary>
	///		GetUpVoteTopText method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string</returns>
	private string GetUpVoteTopText(CommentModel comment)
	{
		if (comment.UserVotes?.Count > 0)
		{
			return comment.UserVotes.Count.ToString("00");
		}

		return comment.Author.Id == _loggedInUser?.Id ? "Awaiting" : "Click To";
	}

	/// <summary>
	///		GetUpVoteBottomText method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string</returns>
	private string GetUpVoteBottomText(CommentModel comment)
	{
		return comment.UserVotes?.Count > 1 ? "UpVotes" : "UpVote";
	}

	/// <summary>
	///		GetVoteCssClass method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string css class</returns>
	private string GetVoteCssClass(CommentModel comment)
	{
		if (comment.UserVotes is null || comment.UserVotes.Count == 0)
		{
			return "issue-detail-no-votes";
		}

		return comment.UserVotes.Contains(_loggedInUser?.Id) ? "issue-detail-voted" : "issue-detail-not-voted";
	}

	/// <summary>
	///		OpenCommentForm method
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
	///		ClosePage method
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}