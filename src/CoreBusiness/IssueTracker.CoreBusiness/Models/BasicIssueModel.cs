﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     BasicIssueModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.Models;

/// <summary>
///   BasicIssueModel class
/// </summary>
[Serializable]
public class BasicIssueModel
{
	/// <summary>
	///   Initializes a new instance of the <see cref="BasicIssueModel" /> class.
	/// </summary>
	public BasicIssueModel()
	{
	}

	/// <summary>
	///   Initializes a new instance of the <see cref="BasicIssueModel" /> class.
	/// </summary>
	/// <param name="issue">The issue.</param>
	public BasicIssueModel(IssueModel issue)
	{
		Id = issue.Id;
		Title = issue.Title;
		Description = issue.Description;
		DateCreated = issue.DateCreated;
		Category = issue.Category;
		Status = issue.IssueStatus;
		Author = issue.Author;
	}

	/// <summary>
	///   Gets or sets the identifier.
	/// </summary>
	/// <value>
	///   The identifier.
	/// </value>
	public string Id { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the title.
	/// </summary>
	/// <value>
	///   The title.
	/// </value>
	public string Title { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the description.
	/// </summary>
	/// <value>
	///   The description.
	/// </value>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// </summary>
	public DateTime DateCreated { get; set; } = DateTime.UtcNow;


	/// <summary>
	///   Gets or sets the author.
	/// </summary>
	/// <value>
	///   The author.
	/// </value>
	public BasicUserModel Author { get; set; } = new();

	/// <summary>
	///   Gets or sets the category.
	/// </summary>
	/// <value>
	///   The category.
	/// </value>
	public BasicCategoryModel Category { get; set; } = new();

	/// <summary>
	///   Gets or sets the status.
	/// </summary>
	/// <value>
	///   The status.
	/// </value>
	public BasicStatusModel Status { get; set; } = new();
}