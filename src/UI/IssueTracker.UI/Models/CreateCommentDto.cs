//-----------------------------------------------------------------------
// <copyright file="CreateCommentDto.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Models;

public class CreateCommentDto
{
	[Required] [MaxLength(75)] public string? Title { get; set; }

	[Required] [MaxLength(500)] public string? Description { get; set; }
}
