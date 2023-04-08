global using System.Diagnostics.CodeAnalysis;
global using System.Net;

global using FluentAssertions;

global using IssueTracker.CoreBusiness.BogusFakes;
global using IssueTracker.CoreBusiness.Models;
global using IssueTracker.PlugIns.Mongo.Contracts;
global using IssueTracker.PlugIns.Mongo.Helpers;
global using IssueTracker.PlugIns.PlugInRepositoryInterfaces;
global using IssueTracker.PlugIns.Services;
global using IssueTracker.UI;

global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;

global using Xunit;

