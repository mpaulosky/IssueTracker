//-----------------------------------------------------------------------
// <copyright file="RegisterServices.cs" company="mpaulosky">
//		Author: Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Extensions;

/// <summary>
///		RegisterServices class
/// </summary>
[ExcludeFromCodeCoverage]
public static class RegisterServices
{

	///  <summary>
	/// 		Configures the services method.
	///  </summary>
	///  <param name="builder">The builder.</param>
	///  <param name="config">ConfigurationManager</param>
	public static void ConfigureServices(this WebApplicationBuilder builder, ConfigurationManager config)
	{

		// Add services to the container.

		builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();

		builder.Services.AddAuthorizationService();

		builder.Services.AddAuthenticationService(config);

		builder.Services.AddRazorPages();

		builder.Services.AddMemoryCache();

		builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

		builder.Services.AddBlazoredSessionStorage();

		builder.Services.RegisterDatabaseContext(config);

		builder.Services.RegisterDICollections();

	}

}
