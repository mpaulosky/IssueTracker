//-----------------------------------------------------------------------
// <copyright file="CreateIssueModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Models;

public class CreateCategoryDto
{

	[Required]
	[MinLength(3)]
	[MaxLength(75)]
	[Display(Name = "Name")]
	public string CategoryNaem { get; set; }

	[Required]
	[MinLength(3)]
	[MaxLength(200)]
	[Display(Name ="Description")]
	public string CategoryDescription { get; set; }

}