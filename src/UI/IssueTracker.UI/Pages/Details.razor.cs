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
[UsedImplicitly]
public partial class Details
{

	private List<CommentModel>? _comments = new();

	private IssueModel? _issue = new();

	private UserModel? _loggedInUser = new();
	private string? _settingStatus;
	private List<StatusModel> _statuses = new();
	private string? _urlText;

	[Parameter] public string? Id { get; set; }

	/// <summary>
	///		OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{

		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);

		ArgumentNullException.ThrowIfNull(Id);

		_issue = await IssueService.GetIssue(Id);
		var source = new BasicCommentOnSourceModel(_issue);
		_comments = await CommentService.GetCommentsBySource(source);
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

				if (string.IsNullOrWhiteSpace(_urlText)) return;

				StatusModel selectedStatus = _statuses.First(s => string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue!.IssueStatus = new BasicStatusModel(selectedStatus.StatusName, selectedStatus.StatusDescription);

				break;

			case "in work":

				_issue!.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));

				break;

			case "watching":

				_issue!.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));

				break;

			case "dismissed":

				_issue!.IssueStatus = new BasicStatusModel(_statuses.First(s =>
					string.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase)));

				break;

			default:

				return;

		}

		_settingStatus = null;

		await IssueService.UpdateIssue(_issue);

	}

	/// <summary>
	///		OpenCommentForm method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	private void OpenCommentForm(IssueModel issue)
	{

		if (_loggedInUser is not null) NavManager.NavigateTo($"/Comment/{issue.Id}");
	}

	/// <summary>
	///		ClosePage method
	/// </summary>
	private void ClosePage()
	{

		NavManager.NavigateTo("/");

	}

}
