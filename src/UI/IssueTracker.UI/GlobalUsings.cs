﻿// Global using directives

global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;

global using Blazored.SessionStorage;

global using IssueTracker.CoreBusiness.BogusFakes;
global using IssueTracker.CoreBusiness.Contracts;
global using IssueTracker.CoreBusiness.Models;
global using IssueTracker.PlugIns.Contracts;
global using IssueTracker.PlugIns.DataAccess;
global using IssueTracker.Services.Category.Interface;
global using IssueTracker.Services.Comment.Interface;
global using IssueTracker.Services.Issue.Interface;
global using IssueTracker.Services.PlugInRepositoryInterfaces;
global using IssueTracker.Services.Status.Interface;
global using IssueTracker.Services.User.Interface;
global using IssueTracker.UI.Extensions;
global using IssueTracker.UI.Helpers;
global using IssueTracker.UI.Models;
global using IssueTracker.UseCases.Solution;

global using JetBrains.Annotations;

global using Microsoft.AspNetCore.Authentication.OpenIdConnect;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Authorization;
global using Microsoft.AspNetCore.Rewrite;
global using Microsoft.Identity.Web;
global using Microsoft.Identity.Web.UI;
