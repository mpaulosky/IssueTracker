namespace IssueTracker.UI.Shared;

public partial class IssueComponent
{
	[Parameter]
	public bool CanArchive { get; set; } = false;
	[Parameter]
	public UserModel LoggedInUser { get; set; } = new();
	[Parameter]
	public IssueModel Item { get; set; } = new();

	private IssueModel _archivingIssue = new();

	/// <summary>
	///		GetIssueCategoryCssClass
	/// </summary>
	/// <param name = "issue">IssueModel</param>
	/// <returns>string css class</returns>
	private static string GetIssueCategoryCssClass(IssueModel issue)
	{
		if (issue.Category is null)
		{
			return "issue-entry-category-none";
		}

		var output = issue.Category.CategoryName switch
		{
			"Design" => "issue-entry-category-design",
			"Documentation" => "issue-entry-category-documentation",
			"Implementation" => "issue-entry-category-implementation",
			"Clarification" => "issue-entry-category-clarification",
			"Miscellaneous" => "issue-entry-category-miscellaneous",
			_ => "issue-entry-category-none"
		};
		return output;
	}

	/// <summary>
	///		GetIssueStatusCssClass method
	/// </summary>
	/// <param name = "issue">IssueModel</param>
	/// <returns>string css class</returns>
	private static string GetIssueStatusCssClass(IssueModel issue)
	{
		if (issue.IssueStatus is null)
		{
			return "issue-entry-status-none";
		}

		var output = issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-entry-status-answered",
			"In Work" => "issue-entry-status-inwork",
			"Watching" => "issue-entry-status-watching",
			"Dismissed" => "issue-entry-status-dismissed",
			_ => "issue-entry-status-none"
		};
		return output;
	}

	/// <summary>
	///		OpenDetailsPage method
	/// </summary>
	/// <param name = "issue">IssueModel</param>
	private void OpenDetailsPage(IssueModel issue)
	{
		NavManager.NavigateTo($"/Details/{issue.Id}");
	}

	/// <summary>
	///		Archive issue method
	/// </summary>
	private async Task ArchiveIssue()
	{
		_archivingIssue.Archived = true;
		await IssueService.UpdateIssue(_archivingIssue);
		_archivingIssue = null;
	}
}