global using FluentAssertions;

global using IssueTracker.Library.Contracts;
global using IssueTracker.Library.Helpers;
global using IssueTracker.Library.Helpers.BogusFakes;
global using IssueTracker.Library.Models;
global using IssueTracker.Library.Services;

global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Options;

global using MongoDB.Driver;

global using Moq;

global using NSubstitute;

global using System.Diagnostics.CodeAnalysis;

global using TestingSupport.Library.Fixtures;

global using Xunit;

global using static IssueTracker.Library.Helpers.CollectionNames;