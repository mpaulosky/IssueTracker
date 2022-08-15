namespace IssueTracker.UI.Pages;

/// <summary>
/// Create class
/// </summary>
public partial class Create
{
	private CreateIssueModel _issue = new();
	private List<CategoryModel> _categories;
	private UserModel _loggedInUser;
	
	/// <summary>
	/// OnInitializedAsync method
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		_categories = await CategoryService.GetCategories();
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
	}

	/// <summary>
	/// ClosePage method
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}

	/// <summary>
	/// CreateIssue method
	/// </summary>
	private async Task CreateIssue()
	{
		IssueModel s = new()
			{
				IssueName = _issue.Issue,
				Description = _issue.Description,
				Author = new BasicUserModel(_loggedInUser),
				Category = _categories.FirstOrDefault(c => c.Id == _issue.CategoryId)
			};
		
		if (s.Category is null)
		{
			_issue.CategoryId = "";
			return;
		}

		await IssueService.CreateIssue(s);

		_issue = new CreateIssueModel();
		ClosePage();
		
	}
}