// Global using directives

global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.Linq;
global using System.Net;
global using System.Security.Claims;
global using System.Threading.Tasks;

global using AngleSharp.Dom;

global using Blazored.SessionStorage;

global using Bunit;

global using FluentAssertions;

global using IssueTracker.CoreBusiness.BogusFakes;
global using IssueTracker.CoreBusiness.Models;
global using IssueTracker.PlugIns.Contracts;
global using IssueTracker.Services.Category;
global using IssueTracker.Services.Category.Interface;
global using IssueTracker.Services.Comment;
global using IssueTracker.Services.Comment.Interface;
global using IssueTracker.Services.Issue;
global using IssueTracker.Services.Issue.Interface;
global using IssueTracker.Services.PlugInRepositoryInterfaces;
global using IssueTracker.Services.Status;
global using IssueTracker.Services.Status.Interface;
global using IssueTracker.Services.User;
global using IssueTracker.Services.User.Interface;
global using IssueTracker.UI.Components;
global using Microsoft.Extensions.Caching.Memory;

global using Moq;
global using Moq.Protected;
global using TestingSupport.Library.Fixtures;

global using Xunit;
