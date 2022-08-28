//-----------------------------------------------------------------------
// <copyright file="BasicIssueModel.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
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
	}

	public string Id { get; set; }

	public string Issue { get; set; }

	public string Description { get; set; }
}