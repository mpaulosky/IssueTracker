//-----------------------------------------------------------------------
// <copyright File="BasicSolutionModel.cs"
//	Company="mpaulosky">
//	Author: Matthew Paulosky
//	Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.CoreBusiness.Models;

public class BasicSolutionModel
{

	public BasicSolutionModel()
	{
	}

	public BasicSolutionModel(SolutionModel solution)
	{

		Id = solution.Id;
		SolutionTitle = solution.SolutionTitle;
		Solution = solution.Solution;
		Issue = solution.Issue;
		Author = solution.Author;

	}

	public string Id { get; set; } = string.Empty;

	public string SolutionTitle { get; set; } = string.Empty;

	public string Solution { get; set; } = string.Empty;

	public BasicIssueModel Issue { get; set; } = new BasicIssueModel();

	public BasicUserModel Author { get; set; } = new();

}
