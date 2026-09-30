// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Enums.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.Enum;

public class Enums
{
	/// <summary>
	///   Category enum
	/// </summary>
	internal enum Category
	{
		Design,
		Documentation,
		Implementation,
		Clarification,
		Miscellaneous
	}

	/// <summary>
	///   Status enum: the Status names an Admin can give an approved Issue, as named in CONTEXT.md
	/// </summary>
	internal enum Status
	{
		Accepted,
		Watching,
		Dismissed,
		Upcoming
	}
}