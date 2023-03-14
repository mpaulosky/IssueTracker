//-----------------------------------------------------------------------
// <copyright file="AddAuthorizationAuthenticationServices.cs" company="mpaulosky">
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
	/// Add Authorization Authentication Services
	/// </summary>
	/// <param name="services">IServiceCollection</param>
	/// <param name="config">ConfigurationManager</param>
	/// <returns>IServiceCollection</returns>
	public static IServiceCollection AddAuthorizationAuthenticationServices(this IServiceCollection services, ConfigurationManager config)
	{

		services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(config.GetSection("AzureAdB2C"));

		services.AddAuthorization(options =>
		{
			options.AddPolicy("Admin", policy =>
			{
				policy.RequireClaim("jobTitle", "Admin");
			});
		});

		return services;

	}

}
