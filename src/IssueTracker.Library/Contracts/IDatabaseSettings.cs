//-----------------------------------------------------------------------
// <copyright file="IDatabaseSettings.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.Contracts;

public interface IDatabaseSettings
{
	string ConnectionString { get; set; }

	string DatabaseName { get; set; }
}