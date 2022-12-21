global using FluentAssertions;

global using IssueTracker.Library.Contracts;
global using IssueTracker.Library.DataAccess;
global using IssueTracker.Library.Helpers;
global using IssueTracker.Library.Helpers.BogusFakes;
global using IssueTracker.Library.Models;
global using IssueTracker.UI;

global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;

global using System.Diagnostics.CodeAnalysis;
global using System.Net;

global using Xunit;