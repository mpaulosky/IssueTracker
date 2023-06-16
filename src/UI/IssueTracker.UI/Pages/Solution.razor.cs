// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Solution.razor.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

namespace IssueTracker.UI.Pages;

/// <summary>
///  Solution page class.
/// </summary>
[UsedImplicitly]
public partial class Solution
{
	private CreateSolutionDto _solution = new();
	private IssueModel? _issue;
	private UserModel? _loggedInUser;
	[Parameter] public string? Id { get; set; }

	/// <summary>
	///  OnInitializedAsync event.
	/// </summary>
	/// <returns>Task</returns>
	protected override async Task OnInitializedAsync()
	{
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);
		_issue = await IssueService.GetIssue(Id!);
	}

	/// <summary>
	///  Create Solution method.
	/// </summary>
	/// <returns>Task</returns>
	private async Task CreateSolution()
	{
		SolutionModel solution = new()
		{
			Author = new BasicUserModel(_loggedInUser!),
			Title = _solution.Title!,
			Description = _solution.Description!,
			Issue = new BasicIssueModel(_issue!)
		};

		await SolutionService.CreateSolution(solution);

		_solution = new CreateSolutionDto();

		ClosePage();
	}

	/// <summary>
	///   OpenCommentForm method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	private void OpenCommentForm(IssueModel issue)
	{
		if (_loggedInUser is not null)
		{
			NavManager.NavigateTo($"/Comment/{issue.Id}");
		}
	}

	/// <summary>
	///   ClosePage method.
	/// </summary>
	private void ClosePage()
	{
		NavManager.NavigateTo("/");
	}
}