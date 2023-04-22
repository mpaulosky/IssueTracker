//-----------------------------------------------------------------------
// <copyright file="RegisterDICollections.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

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
