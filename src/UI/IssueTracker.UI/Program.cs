// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.UI
// =============================================

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ServiceDefaults: Health checks, OTel, problem details
builder.AddServiceDefaults();

ConfigurationManager config = builder.Configuration;
config.AddEnvironmentVariables("IssueTrackerUI_");

// Add services to the container.
builder.ConfigureServices(config);

WebApplication app = builder.Build();

// Map health check and default endpoints
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseRewriter(
	new RewriteOptions().Add(
		context =>
		{
			if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
			{
				context.HttpContext.Response.Redirect("/");
			}
		}
	));

app.MapControllers();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();