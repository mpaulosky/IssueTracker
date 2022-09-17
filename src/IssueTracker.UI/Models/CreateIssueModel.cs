//-----------------------------------------------------------------------
// <copyright file="CreateIssueModel.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Models;

public class CreateIssueModel
{
	[Required][MaxLength(75)] public string Issue { get; set; }

	[Required]
	[MinLength(1)]
	[Display(Name = "Category")]
	public string CategoryId { get; set; }

	[Required][MaxLength(500)] public string Description { get; set; }
}