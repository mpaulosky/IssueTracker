//-----------------------------------------------------------------------
// <copyright file="NotAuthorized.cs" company="mpaulosky">
//     Author: Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.UI.Shared;

/// <summary>
/// NotAuthorized class
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase"/>
public partial class NotAuthorized
{
	/// <summary>
	/// Closes the page method.
	/// </summary>
	private void ClosePage() { NavManager.NavigateTo("/"); }
}