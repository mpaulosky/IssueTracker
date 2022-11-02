namespace IssueTracker.UI.Pages;

public partial class Categories
{

	private UserModel _loggedInUser;
	private List<CategoryModel> _categories = new();
	private CategoryModel _editCategory;
	private CategoryModel _deleteCategory;


	/// <summary>
	///		OnInitializedAsync event.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{

		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
		_categories = (await CategoryService.GetCategories());

	}

	private void NewCategory()
	{
		_categories.Add(new CategoryModel { });
	}

	/// <summary>
	///		ClosePage method.
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}

}