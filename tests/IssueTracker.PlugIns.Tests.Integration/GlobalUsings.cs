// Global using directives

global using System.Diagnostics.CodeAnalysis;
global using System.Net;

global using FluentAssertions;

global using IssueTracker.CoreBusiness.BogusFakes;
global using IssueTracker.CoreBusiness.Helpers;
global using IssueTracker.CoreBusiness.Models;
global using IssueTracker.PlugIns.Contracts;
global using IssueTracker.PlugIns.DataAccess;
global using IssueTracker.Services.PlugInRepositoryInterfaces;
global using IssueTracker.UI;
global using JetBrains.Annotations;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;

global using MongoDB.Driver;

global using Testcontainers.MongoDb;

global using Xunit;
