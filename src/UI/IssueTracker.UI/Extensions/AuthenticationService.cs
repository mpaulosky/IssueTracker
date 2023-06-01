﻿//-----------------------------------------------------------------------
// <copyright file="AddAuthorizationAuthenticationServices.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Extensions;

/// <summary>
///   IServiceCollectionExtensions
/// </summary>
public static partial class ServiceCollectionExtensions
{
	/// <summary>
	///   Add Authentication Services
	/// </summary>
	/// <param name="services">IServiceCollection</param>
	/// <param name="config">ConfigurationManager</param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection AddAuthenticationService(this IServiceCollection services,
		ConfigurationManager config)
	{
		services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(config.GetSection("AzureAdB2C"));

		return services;
	}
}
