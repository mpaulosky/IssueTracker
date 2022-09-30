//-----------------------------------------------------------------------
// <copyright file="SampleData.razor.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022.2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Pages;

/// <summary>
///		SampleData class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
[ExcludeFromCodeCoverage]
public partial class SampleData
{
	private bool _usersCreated;
	private bool _categoriesCreated;
	private bool _statusesCreated;
	private bool _commentsCreated;
	private bool _issuesCreated;

	protected override async Task OnInitializedAsync()
	{
		await SetButtonStatus();
	}

	private async Task SetButtonStatus()
	{
		_usersCreated = (await UserService.GetUsers()).Any();
		_categoriesCreated = (await CategoryService.GetCategories()).Any();
		_statusesCreated = (await StatusService.GetStatuses()).Any();
		_commentsCreated = (await CommentService.GetComments()).Any();
		_issuesCreated = (await IssueService.GetIssues()).Any();
	}

	/// <summary>
	/// Creates the Users method.
	/// </summary>
	private async Task CreateUsers()
	{
		var users = await UserService.GetUsers();

		if (users?.Count > 0)
		{
			return;
		}

		var items = FakeUser.GetUsers(2);

		foreach (var item in items)
		{
			await UserService.CreateUser(item);
		}
		_usersCreated = true;
	}

	/// <summary>
	///		Creates the categories method.
	/// </summary>
	private async Task CreateCategories()
	{
		var categories = await CategoryService.GetCategories();

		if (categories?.Count > 0)
		{
			return;
		}

		CategoryModel item = new()
		{
			CategoryName = "Design",
			CategoryDescription = "An Issue with the design."
		};
		await CategoryService.CreateCategory(item);

		item = new CategoryModel
		{
			CategoryName = "Documentation",
			CategoryDescription = "An Issue with the documentation."
		};
		await CategoryService.CreateCategory(item);

		item = new CategoryModel
		{
			CategoryName = "Implementation",
			CategoryDescription = "An Issue with the implementation."
		};
		await CategoryService.CreateCategory(item);

		item = new CategoryModel
		{
			CategoryName = "Clarification",
			CategoryDescription = "A quick Issue with a general question."
		};
		await CategoryService.CreateCategory(item);

		item = new CategoryModel
		{
			CategoryName = "Miscellaneous",
			CategoryDescription = "Not sure where this fits."
		};
		await CategoryService.CreateCategory(item);

		_categoriesCreated = true;
	}

	/// <summary>
	///		Creates the statuses method.
	/// </summary>
	private async Task CreateStatuses()
	{
		var statuses = await StatusService.GetStatuses();

		if (statuses?.Count > 0)
		{
			return;
		}

		StatusModel item = new()
		{
			StatusName = "Answered",
			StatusDescription = "The suggestion was accepted and the corresponding item was created."
		};
		await StatusService.CreateStatus(item);

		item = new StatusModel
		{
			StatusName = "Watching",
			StatusDescription =
				"The suggestion is interesting. We are watching to see how much interest there is in it."
		};
		await StatusService.CreateStatus(item);

		item = new StatusModel
		{
			StatusName = "Upcoming",
			StatusDescription = "The suggestion was accepted and it will be released soon."
		};
		await StatusService.CreateStatus(item);

		item = new StatusModel
		{
			StatusName = "Dismissed",
			StatusDescription = "The suggestion was not something that we are going to undertake."
		};
		await StatusService.CreateStatus(item);

		_statusesCreated = true;
	}

	/// <summary>
	///		Creates the comments method.
	/// </summary>
	private async Task CreateComments()
	{
		var comments = await CommentService.GetComments();

		if (comments?.Count > 0)
		{
			return;
		}

		var items = FakeComment.GetComments(4);

		foreach (var item in items)
		{
			await CommentService.CreateComment(item);
		}
		_commentsCreated = true;
	}

	/// <summary>
	/// Creates Issues method.
	/// </summary>
	private async Task CreateIssues()
	{
		var issues = await IssueService.GetIssues();

		if (issues?.Count > 0)
		{
			return;
		}

		var items = FakeIssue.GetIssues(6);

		foreach (var issue in items)
		{
			await IssueService.CreateIssue(issue);
		}
		_issuesCreated = true;
	}
}