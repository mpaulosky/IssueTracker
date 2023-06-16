﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RegisterServicesCollections.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

using IssueTracker.Services.Category;
using IssueTracker.Services.Comment;
using IssueTracker.Services.Issue;
using IssueTracker.Services.Solution;
using IssueTracker.Services.Status;
using IssueTracker.Services.User;

namespace IssueTracker.UI.Extensions;

/// <summary>
///   IServiceCollectionExtensions
/// </summary>
public static partial class ServiceCollectionExtensions
{
	/// <summary>
	///   Register DI Collections
	/// </summary>
	/// <param name="services">IServiceCollection</param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection RegisterServicesCollections(this IServiceCollection services)
	{
		services.AddSingleton<ICategoryService, CategoryService>();
		services.AddSingleton<ICommentService, CommentService>();
		services.AddSingleton<IStatusService, StatusService>();
		services.AddSingleton<IIssueService, IssueService>();
		services.AddSingleton<IUserService, UserService>();
		services.AddSingleton<ISolutionService, SolutionService>();

		return services;
	}
}