//-----------------------------------------------------------------------
// <copyright file="CreateCommentModel.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Models;

public class CreateCommentModel
{
	[Required] [MaxLength(500)] public string Comment { get; set; }
}