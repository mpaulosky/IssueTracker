namespace IssueTracker.UI.Pages;

public partial class Index
{
	private UserModel _loggedInUser;
	private List<IssueModel> _issues;
	private List<CategoryModel> _categories;
	private List<StatusModel> _statuses;
	private IssueModel _archivingIssue;
	private string _selectedCategory = "All";
	private string _selectedStatus = "All";
	private string _searchText = "";
	private bool _isSortedByNew = true;
	private bool _showCategories;
	private bool _showStatuses;

	protected override async Task OnInitializedAsync()
	{
		_categories = await CategoryService.GetCategories();
		_statuses = await StatusService.GetStatuses();
		await LoadAndVerifyUser();
	}

	private async Task ArchiveSuggestion()
	{
		_archivingIssue.Archived = true;
		await IssueService.UpdateIssue(_archivingIssue);
		_issues.Remove(_archivingIssue);
		_archivingIssue = null;
		//await FilterSuggestions();
	}

	private void LoadCreatePage()
	{
		if (_loggedInUser is not null)
		{
			NavManager.NavigateTo("/Create");
		}
		else
		{
			NavManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
		}
	}

	private async Task LoadAndVerifyUser()
	{
		var authState = await AuthProvider.GetAuthenticationStateAsync();
		string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
		if (string.IsNullOrWhiteSpace(objectId) == false)
		{
			_loggedInUser = await UserService.GetUserFromAuthentication(objectId) ?? new();
			string firstName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("givenname"))?.Value;
			string lastName = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value;
			string displayName = authState.User.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
			string email = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value;
			bool isDirty = false;

			if (objectId.Equals(_loggedInUser.ObjectIdentifier) == false)
			{
				isDirty = true;
				_loggedInUser.ObjectIdentifier = objectId;
			}

			if (firstName?.Equals(_loggedInUser.FirstName) == false)
			{
				isDirty = true;
				_loggedInUser.FirstName = firstName;
			}

			if (lastName?.Equals(_loggedInUser.LastName) == false)
			{
				isDirty = true;
				_loggedInUser.LastName = lastName;
			}

			if (displayName?.Equals(_loggedInUser.DisplayName) == false)
			{
				isDirty = true;
				_loggedInUser.DisplayName = displayName;
			}

			if (email?.Equals(_loggedInUser.EmailAddress) == false)
			{
				isDirty = true;
				_loggedInUser.EmailAddress = email;
			}

			if (isDirty)
			{
				if (string.IsNullOrWhiteSpace(_loggedInUser.Id))
				{
					await UserService.CreateUser(_loggedInUser);
				}
				else
				{
					await UserService.UpdateUser(_loggedInUser);
				}
			}
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await LoadFilterState();
			await FilterSuggestions();
			StateHasChanged();
		}
	}

	private async Task LoadFilterState()
	{
		var stringResults = await SessionStorage.GetAsync<string>(nameof(_selectedCategory));
		_selectedCategory = stringResults.Success ? stringResults.Value : "All";
		stringResults = await SessionStorage.GetAsync<string>(nameof(_selectedStatus));
		_selectedStatus = stringResults.Success ? stringResults.Value : "All";
		stringResults = await SessionStorage.GetAsync<string>(nameof(_searchText));
		_searchText = stringResults.Success ? stringResults.Value : "";
		var boolResults = await SessionStorage.GetAsync<bool>(nameof(_isSortedByNew));
		_isSortedByNew = !boolResults.Success || boolResults.Value;
	}

	private async Task SaveFilterState()
	{
		await SessionStorage.SetAsync(nameof(_selectedCategory), _selectedCategory);
		await SessionStorage.SetAsync(nameof(_selectedStatus), _selectedStatus);
		await SessionStorage.SetAsync(nameof(_searchText), _searchText);
		await SessionStorage.SetAsync(nameof(_isSortedByNew), _isSortedByNew);
	}

	private async Task FilterSuggestions()
	{
		var output = await IssueService.GetApprovedIssues();
		if (_selectedCategory != "All")
		{
			output = output.Where(s => s.Category?.CategoryName == _selectedCategory).ToList();
		}

		if (_selectedStatus != "All")
		{
			output = output.Where(s => s.IssueStatus?.StatusName == _selectedStatus).ToList();
		}

		if (string.IsNullOrWhiteSpace(_searchText) == false)
		{
			output = output.Where(s =>
			s.IssueName.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase) || 
			s.Description.Contains(_searchText, StringComparison.InvariantCultureIgnoreCase))
				.ToList();
		}
		
		if (_isSortedByNew)
		{
			output = output.OrderByDescending(s => s.DateCreated).ToList();
		}
		// else
		// {
		// 	output = output.OrderByDescending(s => s.UserVotes.Count).ThenByDescending(s => s.DateCreated).ToList();
		// }

		_issues = output;
		await SaveFilterState();
	}

	private async Task OrderByNew(bool isNew)
	{
		_isSortedByNew = isNew;
		await FilterSuggestions();
	}

	private async Task OnSearchInput(string searchInput)
	{
		_searchText = searchInput;
		await FilterSuggestions();
	}

	private async Task OnCategoryClick(string category = "All")
	{
		_selectedCategory = category;
		_showCategories = false;
		await FilterSuggestions();
	}

	private async Task OnStatusClick(string status = "All")
	{
		_selectedStatus = status;
		_showStatuses = false;
		await FilterSuggestions();
	}

	// private async Task VoteUp(IssueModel issue)
	// {
	// 	if (_loggedInUser is not null)
	// 	{
	// 		if (issue.Author.Id == _loggedInUser.Id)
	// 		{
	// 			// Can't vote on your own suggestion
	// 			return;
	// 		}
	//
	// 		if (issue.UserVotes.Add(_loggedInUser.Id) == false)
	// 		{
	// 			suggestion.UserVotes.Remove(_loggedInUser.Id);
	// 		}
	//
	// 		await IssueService.UpvoteSuggestion(suggestion.Id, _loggedInUser.Id);
	// 		if (_isSortedByNew == false)
	// 		{
	// 			_issues = _issues.OrderByDescending(s => s.UserVotes.Count).ThenByDescending(s => s.DateCreated).ToList();
	// 		}
	// 	}
	// 	else
	// 	{
	// 		NavManager.NavigateTo("/MicrosoftIdentity/Account/SignIn", true);
	// 	}
	// }

	// private string GetUpvoteTopText(IssueModel issue)
	// {
	// 	if (issue.UserVotes?.Count > 0)
	// 	{
	// 		return issue.UserVotes.Count.ToString("00");
	// 	}
	// 	else
	// 	{
	// 		if (issue.Author.Id == _loggedInUser?.Id)
	// 		{
	// 			return "Awaiting";
	// 		}
	// 		else
	// 		{
	// 			return "Click To";
	// 		}
	// 	}
	// }

	// private string GetUpvoteBottomText(IssueModel issue)
	// {
	// 	if (issue.UserVotes?.Count > 1)
	// 	{
	// 		return "Upvotes";
	// 	}
	// 	else
	// 	{
	// 		return "Upvote";
	// 	}
	// }

	private void OpenDetails(IssueModel issue)
	{
		NavManager.NavigateTo($"/Details/{issue.Id}");
	}

	private string SortedByNewClass(bool isNew)
	{
		if (isNew == _isSortedByNew)
		{
			return "sort-selected";
		}
		else
		{
			return "";
		}
	}

	// private string GetVoteClass(IssueModel issue)
	// {
	// 	if (issue.UserVotes is null || issue.UserVotes.Count == 0)
	// 	{
	// 		return "suggestion-entry-no-votes";
	// 	}
	// 	else if (issue.UserVotes.Contains(_loggedInUser?.Id))
	// 	{
	// 		return "suggestion-entry-voted";
	// 	}
	// 	else
	// 	{
	// 		return "suggestion-entry-not-voted";
	// 	}
	// }

	private string GetSuggestionStatusClass(IssueModel issue)
	{
		if (issue is null | issue?.IssueStatus is null)
		{
			return "suggestion-entry-status-none";
		}

		string output = issue.IssueStatus.StatusName switch
		{
			"Answered" => "issue-entry-status-answered",
			"In Work" => "issue-entry-status-inwork",
			"Watching" => "issue-entry-status-watching",
			"Dismissed" => "issue-entry-status-dismissed",
			_ => "issue-entry-status-none",
		};
		return output;
	}

	private string GetSelectedCategory(string category = "All")
	{
		return category == _selectedCategory ? "selected-category" : "";
	}

	private string GetSelectedStatus(string status = "All")
	{
		return status == _selectedStatus ? "selected-status" : "";
	}
}