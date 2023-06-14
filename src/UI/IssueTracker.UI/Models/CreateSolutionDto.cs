// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     CreateSolutionDto.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

namespace IssueTracker.UI.Models;

/// <summary>
///   CreateSolutionDto class
/// </summary>
public class CreateSolutionDto
{
	[Required] [MaxLength(75)] public string? SolutionTitle { get; set; }
	[Required] [MaxLength(500)] public string? SolutionDescription { get; set; }
	[Required] public string? IssueId { get; set; }
}