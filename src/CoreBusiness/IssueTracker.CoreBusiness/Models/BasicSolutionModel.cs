// Copyright (c) 2023. All rights reserved.
// File Name :     BasicSolutionModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   BasicSolutionModel class
/// </summary>
[Serializable]
public class BasicSolutionModel
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicSolutionModel" /> class.
	/// </summary>
	/// <param name="solution">The solution.</param>
	public BasicSolutionModel(SolutionModel solution)
	{
		Id = solution.Id;
		Title = solution.Title;
		Description = solution.Description;
		Issue = solution.Issue;
		Author = solution.Author;
	}

	/// <summary>
	///   Gets or sets the identifier.
	/// </summary>
	/// <value>
	///   The identifier.
	/// </value>
	public string Id { get; set; }

	/// <summary>
	///   Gets or sets the title.
	/// </summary>
	/// <value>
	///   The title.
	/// </value>
	public string Title { get; set; }

	/// <summary>
	///   Gets or sets the description.
	/// </summary>
	/// <value>
	///   The description.
	/// </value>
	public string Description { get; set; }

	/// <summary>
	///   Gets or sets the issue.
	/// </summary>
	/// <value>
	///   The issue.
	/// </value>
	public BasicIssueModel Issue { get; set; }

	/// <summary>
	///   Gets or sets the author.
	/// </summary>
	/// <value>
	///   The author.
	/// </value>
	public BasicUserModel Author { get; set; }
}