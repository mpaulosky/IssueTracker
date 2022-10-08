//-----------------------------------------------------------------------
// <copyright file="BasicIssueModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Models;

[Serializable]
public class BasicIssueModel
{

	public BasicIssueModel()
	{
	}

	public BasicIssueModel(IssueModel issue)
	{

		Id = issue?.Id;
		Issue = issue?.IssueName;
		Description = issue?.Description;
		Category = issue?.Category;
		Status = issue?.IssueStatus;
		Author = issue?.Author;

	}

	public string? Id { get; set; }

	public string? Issue { get; set; }

	public string? Description { get; set; }

	public BasicCategoryModel? Category { get; set; }

	public BasicStatusModel? Status { get; set; }

	public BasicUserModel? Author { get; set; }

}