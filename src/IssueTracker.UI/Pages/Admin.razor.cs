namespace IssueTracker.UI.Pages;

public partial class Admin
{
	private List<IssueModel> _issues;
	private IssueModel _editingModel;
	private string _currentEditingTitle = "";
	private string _editedTitle = "";
	private string _currentEditingDescription = "";
	private string _editedDescription = "";

	
	protected override async Task OnInitializedAsync()
	{
		_issues = await IssueService.GetIssuesWaitingForApproval();
	}

	private async Task ApproveSubmission(IssueModel issue)
	{
		issue.ApprovedForRelease = true;
		_issues.Remove(issue);
		await IssueService.UpdateIssue(issue);
	}

	private async Task RejectSubmission(IssueModel issue)
	{
		issue.Rejected = true;
		_issues.Remove(issue);
		await IssueService.UpdateIssue(issue);
	}

	private void EditTitle(IssueModel model)
	{
		_editingModel = model;
		_editedTitle = model.IssueName;
		_currentEditingTitle = model.Id;
		_currentEditingDescription = "";
	}

	private async Task SaveTitle(IssueModel model)
	{
		_currentEditingTitle = string.Empty;
		model.IssueName = _editedTitle;
		await IssueService.UpdateIssue(model);
	}

	private void EditDescription(IssueModel model)
	{
		_editingModel = model;
		_editedDescription = model.Description;
		_currentEditingTitle = "";
		_currentEditingDescription = model.Id;
	}

	private async Task SaveDescription(IssueModel model)
	{
		_currentEditingDescription = string.Empty;
		model.Description = _editedDescription;
		await IssueService.UpdateIssue(model);
	}

	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}