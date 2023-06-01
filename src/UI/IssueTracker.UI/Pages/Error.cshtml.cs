//-----------------------------------------------------------------------
// <copyright file="Error.cshtml.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022.2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IssueTracker.UI.Pages;

/// <summary>
///   ErrorModel class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
	public string RequestId { get; set; } = default!;

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	/// <summary>
	///   Called when [get].
	/// </summary>
	public void OnGet() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier; }
}
