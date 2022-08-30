//-----------------------------------------------------------------------
// <copyright file="Error.cshtml.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) .2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Diagnostics;

namespace IssueTracker.UI.Pages;

/// <summary>
///   ErrorModel class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
	private readonly ILogger<ErrorModel> _logger;

	/// <summary>
	///   Initializes a new instance of the <see cref="ErrorModel" /> class.
	/// </summary>
	/// <param name="logger">The logger.</param>
	public ErrorModel(ILogger<ErrorModel> logger) { _logger = logger; }


	public string RequestId { get; set; }

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	/// <summary>
	///   Called when [get].
	/// </summary>
	public void OnGet() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier; }
}