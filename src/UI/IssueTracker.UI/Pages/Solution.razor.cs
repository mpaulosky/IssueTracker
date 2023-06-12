﻿// Copyright (c) 2023. All rights reserved.
// File Name :     Solution.razor.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI

namespace IssueTracker.UI.Pages;

[UsedImplicitly]
public partial class Solution
{
	private IssueModel? _issue;
	private UserModel? _loggedInUser;
	private readonly CreateSolutionDto _solution = new();
	[Parameter] public string? Id { get; set; }

	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
		_issue = await IssueService.GetIssue(Id!);
	}

	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}