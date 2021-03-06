using Microsoft.AspNetCore.Components;
using IssueTracker.UI.Helpers;

namespace IssueTracker.UI.Pages;

public partial class Details
{
	[Parameter] public string Id { get; set; }

	private IssueModel _issue;
	private List<StatusModel> _statuses;
	private string _settingStatus = "";
	private string _urlText = "";

	protected override async Task OnInitializedAsync()
	{
		_issue = await IssueService.GetIssue(Id);
		_statuses = await StatusService.GetStatuses();
	}

	private async Task CompleteSetStatus()
	{
		switch (_settingStatus)
		{
			case "answered":
				if (string.IsNullOrWhiteSpace(_urlText))
				{
					return;
				}

				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue.OwnerNotes =
					$"You are right, this is an important topic for developers. We created a resource about it here: <a href='{_urlText}' target='_blank'>{_urlText}</a>";
				break;
			case "watching":
				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue.OwnerNotes =
					"We noticed the interest this suggestion is getting! If more people are interested we may address this topic in an upcoming resource.";
				break;
			case "dismissed":
				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue.OwnerNotes =
					"Sometimes a good idea doesn�t fit within our scope and vision. This is one of those ideas.";
				break;
			default:
				return;
		}

		_settingStatus = null;
		await IssueService.UpdateIssue(_issue);
	}

	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}

	private string GetStatusClass()
	{
		if (_issue is null | _issue?.IssueStatus is null)
		{
			return "issue-detail-status-none";
		}

		string output = _issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-detail-status-answered",
			"Watching" => "issue-detail-status-watching",
			"Dismissed" => "issue-detail-status-dismissed",
			_ => "issue-detail-status-none",
		};
		return output;
	}
}