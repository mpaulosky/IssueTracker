//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="mpaulosky">
//     Author: Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using IssueTracker.UI;

using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureServices();

var app = builder.Build();

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