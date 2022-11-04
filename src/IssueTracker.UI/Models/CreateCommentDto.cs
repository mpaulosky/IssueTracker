//-----------------------------------------------------------------------
// <copyright file="CreateCommentDto.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Models;

public class CreateCommentDto
{
	[Required][MaxLength(500)] public string Comment { get; set; }

}