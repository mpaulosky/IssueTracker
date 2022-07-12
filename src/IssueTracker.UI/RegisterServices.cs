using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace IssueTracker.UI;

public static class RegisterServices
{
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

		builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
		builder.Services.AddSingleton<ICommentService, CommentService>();
		builder.Services.AddSingleton<IStatusService, StatusService>();
		builder.Services.AddSingleton<IIssueService, IssueService>();
		builder.Services.AddSingleton<IUserService, UserService>();
		builder.Services.AddSingleton<ICommentRepository, CommentRepository>();
		builder.Services.AddSingleton<IStatusRepository, StatusRepository>();
		builder.Services.AddSingleton<IIssueRepository, IssueRepository>();
		builder.Services.AddSingleton<IUserRepository, UserRepository>();
	}
}