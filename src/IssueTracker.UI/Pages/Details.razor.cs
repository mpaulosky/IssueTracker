namespace IssueTracker.UI.Pages;

/// <summary>
/// Details class
/// </summary>
public partial class Details
{
	[Parameter] public string Id { get; set; }
	
	private UserModel _loggedInUser;

	private IssueModel _issue;
	private List<StatusModel> _statuses;
	private List<CommentModel> _comments;
	private string _settingStatus = "";
	private string _urlText = "";

	/// <summary>
	/// OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{

		_issue = await IssueService.GetIssue(Id);
		_comments = await CommentService.GetIssuesComments(Id);
		_statuses = await StatusService.GetStatuses();
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);

	}

	/// <summary>
	/// CompleteSetStatus method
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
				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue.OwnerNotes =
					$"This Issue has a voted answer for it’s solution";
				break;
			case "in work":
				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue.OwnerNotes =
					"There has been an suggested answer for this issue submitted.";
				break;
			case "watching":
				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
				_issue.OwnerNotes =
					"An Issue was submitted requesting help.";
				break;
			case "dismissed":
				_issue.IssueStatus = _statuses.First(s =>
					String.Equals(s.StatusName, _settingStatus, StringComparison.CurrentCultureIgnoreCase));
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
	/// GetStatusClass method
	/// </summary>
	/// <returns>string</returns>
	private string GetStatusClass()
	{

		if (_issue is null | _issue?.IssueStatus is null)
		{
			return "issue-detail-status-none";
		}

		string output = _issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-detail-status-answered",
			"In Work" => "issue-detail-status-inwork",
			"Watching" => "issue-detail-status-watching",
			"Dismissed" => "issue-detail-status-dismissed",
			_ => "issue-detail-status-none",
		};

		return output;

	}

	/// <summary>
	/// VoteUp method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	private async Task VoteUp(CommentModel comment)
	{

		if (_loggedInUser is not null)
		{
			if (comment.Author.Id == _loggedInUser.Id)
			{
				// Can't vote on your own suggestion
				return;
			}

			if (comment.UserVotes.Add(_loggedInUser.Id) == false)
			{
				comment.UserVotes.Remove(_loggedInUser.Id);
			}

			await CommentService.UpvoteComment(comment.Id, _loggedInUser.Id);
		}
		else
		{
			NavManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
		}

	}

	/// <summary>
	/// GetUpVoteTopText method
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
	/// GetUpVoteBottomText method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string</returns>
	private string GetUpVoteBottomText(CommentModel comment)
	{

		return comment.UserVotes?.Count > 1 ? "UpVotes" : "UpVote";

	}
	
	/// <summary>
	/// GetVoteClass method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string</returns>
	private string GetVoteClass(CommentModel comment)
	{

		if (comment.UserVotes is null || comment.UserVotes.Count == 0)
		{
			return "issue-detail-no-votes";
		}

		return comment.UserVotes.Contains(_loggedInUser?.Id) ? "issue-detail-voted" : "issue-detail-not-voted";
	}
	
	/// <summary>
	/// OpenCommentForm method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	private void OpenCommentForm(IssueModel issue)
	{
		NavManager.NavigateTo($"/Comment/{issue.Id}");
	}

	/// <summary>
	/// ClosePage method
	/// </summary>
	private void ClosePage()
	{

		NavManager.NavigateTo("/");

	}

}