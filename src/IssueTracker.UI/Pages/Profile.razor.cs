namespace IssueTracker.UI.Pages;

public partial class Profile
{
	private UserModel _loggedInUser;
	private List<IssueModel> _issues;
	//private List<IssueModel> _approved;
	private List<IssueModel> _archived;
	//private List<IssueModel> _pending;
	//private List<IssueModel> _rejected;
	private List<CommentModel> _comments;

	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
	
		_comments = await CommentService.GetUsersComments(_loggedInUser.Id);
	
		var results = await IssueService.GetUsersIssues(_loggedInUser.Id);
		
		if (results is not null)
		{
			_issues = results.OrderByDescending(s => s.DateCreated).ToList();
		
			//_approved = _issues.Where(s => s.ApprovedForRelease && s.Archived == false & s.Rejected == false).ToList();
		
			_archived = _issues.Where(s => s.Archived && s.Rejected == false).ToList();
		
			//_pending = _issues.Where(s => s.ApprovedForRelease == false && s.Rejected == false).ToList();
		
			//_rejected = _issues.Where(s => s.Rejected).ToList();
		}
	}
	private string GetIssueStatusClass(IssueModel issue)
	{
		if (issue is null | issue?.IssueStatus is null)
		{
			return "issue-profile-status issue-profile-status-none";
		}

		string output = issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-profile-status issue-profile-status-answered",
			"In Work" => "issue-profile-status issue-profile-status-inwork",
			"Watching" => "issue-profile-status issue-profile-status-watching",
			"Dismissed" => "issue-profile-status issue-profile-status-dismissed",
			_ => "issue-profile-status issue-profile-status-none",
		};
		return output;
	}
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}