// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IDatabaseSettings.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness
// =============================================

namespace IssueTracker.CoreBusiness.Contracts;

public interface IDatabaseSettings
{
	string ConnectionStrings { get; set; }

	string DatabaseName { get; set; }
}