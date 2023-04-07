//-----------------------------------------------------------------------
// <copyright file="RegisterDICollections.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
using IssueTracker.PlugIns.Services;
using IssueTracker.PlugIns.Services.Interfaces;

namespace IssueTracker.UI.Extensions;

/// <summary>
/// IServiceCollectionExtensions
/// </summary>
public static partial class IServiceCollectionExtensions
{

	/// <summary>
	/// Register DI Collections
	/// </summary>
	/// <param name="services">IServiceCollection</param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection RegisterDICollections(this IServiceCollection services)
	{

		services.AddSingleton<ICategoryService, CategoryService>();
		services.AddSingleton<ICommentService, CommentService>();
		services.AddSingleton<IStatusService, StatusService>();
		services.AddSingleton<IIssueService, IssueService>();
		services.AddSingleton<IUserService, UserService>();

		services.AddSingleton<ICategoryRepository, CategoryRepository>();
		services.AddSingleton<ICommentRepository, CommentRepository>();
		services.AddSingleton<IStatusRepository, StatusRepository>();
		services.AddSingleton<IIssueRepository, IssueRepository>();
		services.AddSingleton<IUserRepository, UserRepository>();

		return services;

	}

}