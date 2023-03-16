//-----------------------------------------------------------------------
// <copyright file="BasicIssueModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

[Serializable]
public class BasicIssueModel
{

	public BasicIssueModel()
	{
	}

	public BasicIssueModel(IssueModel issue)
	{

		Id = issue.Id;
		IssueTitle = issue.IssueTitle;
		Description = issue.Description;
		Category = issue.Category;
		Status = issue.IssueStatus;
		Author = issue.Author;

	}

	public string Id { get; set; } = string.Empty;

	public string IssueTitle { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public BasicUserModel Author { get; set; } = new();

	public BasicCategoryModel Category { get; set; } = new();

	public BasicStatusModel Status { get; set; } = new();

}