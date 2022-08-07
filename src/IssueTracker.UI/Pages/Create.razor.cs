using IssueTracker.UI.Models;

namespace IssueTracker.UI.Pages;

public partial class Create
{
	private CreateIssueModel _issue = new();
	private List<CategoryModel> _categories;
	private UserModel _loggedInUser;
	
	protected override async Task OnInitializedAsync()
	{
		_categories = await CategoryService.GetCategories();
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
	}

	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}

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