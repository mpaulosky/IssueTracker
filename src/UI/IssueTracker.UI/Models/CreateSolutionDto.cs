﻿//-----------------------------------------------------------------------// <copyright>//	File:		CreateSolutionDto.cs//	Company:mpaulosky//	Author:	Matthew Paulosky//	Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.UI.Models;/// <summary>
///   CreateSolutionDto class
/// </summary>
public class CreateSolutionDto{	[Required][MaxLength(75)] public string? SolutionTitle { get; set; }	[Required][MaxLength(500)] public string? SolutionDescription { get; set; }	[Required] public string? IssueId { get; set; }}
