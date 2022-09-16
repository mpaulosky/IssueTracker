//-----------------------------------------------------------------------
// <copyright file="RegisterServices.cs" company="mpaulosky">
//     Author: Matthew Paulosky
//     Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI;

/// <summary>
///   RegisterServices class
/// </summary>
[ExcludeFromCodeCoverage]
public static class RegisterServices
{
	/// <summary>
	///   Configures the services method.
	/// </summary>
	/// <param name="builder">The builder.</param>
	public static void ConfigureServices(this WebApplicationBuilder builder)
	{
		// Add services to the container.

		builder.Services.Configure<DatabaseSettings>(
			builder.Configuration.GetSection("MongoDbSettings"));

		builder.Services.AddRazorPages();

		builder.Services.AddServerSideBlazor().AddMicrosoftIdentityConsentHandler();

		builder.Services.AddMemoryCache();

		builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

		builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

		builder.Services.AddAuthorization(options =>
		{
			options.AddPolicy("Admin", policy =>
			{
				policy.RequireClaim("jobTitle", "Admin");
			});
		});

		builder.Services.AddBlazoredSessionStorage();

		builder.Services.AddHealthChecks()
			.AddCheck<MongoHealthCheck>("MongoDBConnectionCheck");

		// Setup DI

		builder.Services.AddSingleton<IMongoDbContextFactory, MongoDbContext>();
		builder.Services.AddSingleton<ICategoryService, CategoryService>();
		builder.Services.AddSingleton<ICommentService, CommentService>();
		builder.Services.AddSingleton<IStatusService, StatusService>();
		builder.Services.AddSingleton<IIssueService, IssueService>();
		builder.Services.AddSingleton<IUserService, UserService>();

		builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
		builder.Services.AddSingleton<ICommentRepository, CommentRepository>();
		builder.Services.AddSingleton<IStatusRepository, StatusRepository>();
		builder.Services.AddSingleton<IIssueRepository, IssueRepository>();
		builder.Services.AddSingleton<IUserRepository, UserRepository>();
	}
}