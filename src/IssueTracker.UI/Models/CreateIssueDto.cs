﻿//-----------------------------------------------------------------------
// <copyright file="CreateIssueDto.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Models;

public class CreateIssueDto
{

	[Required][MaxLength(75)] public string? Issue { get; set; }

	[Required][MaxLength(500)] public string? Description { get; set; }

	[Required]
	public string? CategoryId { get; set; }

}