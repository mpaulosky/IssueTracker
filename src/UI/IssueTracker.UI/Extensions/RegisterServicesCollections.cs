//-----------------------------------------------------------------------
// <copyright file="RegisterDICollections.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.Services.Category;
using IssueTracker.Services.Comment;
using IssueTracker.Services.Issue;
using IssueTracker.Services.Issue.Interface;
using IssueTracker.Services.Status;
using IssueTracker.Services.Status.Interface;
using IssueTracker.Services.User;

namespace IssueTracker.UI.Extensions;

/// <summary>
/// IServiceCollectionExtensions
/// </summary>
public static partial class ServiceCollectionExtensions
{

	/// <summary>
	/// Register DI Collections
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

		return services;

	}

}
