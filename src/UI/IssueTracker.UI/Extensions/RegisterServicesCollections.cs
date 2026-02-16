// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     RegisterServicesCollections.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

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
	/// <remarks>
	///   Services are registered as Scoped to avoid captive dependency issues with Transient repositories
	///   and to support proper lifecycle management in Blazor Server per-circuit scenarios.
	/// </remarks>
	public static IServiceCollection RegisterServicesCollections(this IServiceCollection services)
	{
		services.AddScoped<ICategoryService, CategoryService>();
		services.AddScoped<ICommentService, CommentService>();
		services.AddScoped<IStatusService, StatusService>();
		services.AddScoped<IIssueService, IssueService>();
		services.AddScoped<IUserService, UserService>();

		return services;
	}
}
