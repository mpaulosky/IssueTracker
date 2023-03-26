global using System.Diagnostics.CodeAnalysis;

global using FluentAssertions;

global using IssueTracker.CoreBusiness.BogusFakes;
global using IssueTracker.CoreBusiness.Contracts;
global using IssueTracker.CoreBusiness.Models;
global using IssueTracker.CoreBusiness.Services;
global using IssueTracker.PlugIns.Mongo.Contracts;
global using IssueTracker.PlugIns.Mongo.DataAccess;
global using IssueTracker.PlugIns.Mongo.Helpers;

global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Options;

global using MongoDB.Driver;

global using Moq;

global using NSubstitute;

global using TestingSupport.Library.Fixtures;

global using Xunit;

global using static IssueTracker.PlugIns.Mongo.Helpers.CollectionNames;
