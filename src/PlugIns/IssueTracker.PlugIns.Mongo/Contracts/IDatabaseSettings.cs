//-----------------------------------------------------------------------
// <copyright file="IDatabaseSettings.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.PlugIns.Mongo.Contracts;

public interface IDatabaseSettings
{
	string ConnectionString { get; set; }

	string DatabaseName { get; set; }
}