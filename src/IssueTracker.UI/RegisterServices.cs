using IssueTrackerLibrary.Contracts;
using IssueTrackerLibrary.Helpers;
using IssueTrackerLibrary.Services;

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace IssueTrackerUI;

public static class RegisterServices
{
	public static void ConfigureServices(this WebApplicationBuilder builder)
	{

		// Add services to the container.
		
		builder.Services.Configure<IssueTrackerDatabaseSettings>(
			builder.Configuration.GetSection("MongoDbSettings"));

		builder.Services.AddRazorPages();

		builder.Services.AddServerSideBlazor()
			.AddMicrosoftIdentityConsentHandler();

		builder.Services.AddMemoryCache();

		builder.Services.AddControllersWithViews()
			.AddMicrosoftIdentityUI();

		builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

		builder.Services.AddAuthorization(options =>
		{
			options.AddPolicy("Admin", policy =>
			{
				policy.RequireClaim("jobTitle", "Admin");
			});
		});

		builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
		builder.Services.AddSingleton<ICommentService, MongoCommentService>();
		builder.Services.AddSingleton<IStatusService, MongoStatusService>();
		builder.Services.AddSingleton<IIssueService, MongoIssueService>();
		builder.Services.AddSingleton<IUserService, MongoUserService>();

	}
}