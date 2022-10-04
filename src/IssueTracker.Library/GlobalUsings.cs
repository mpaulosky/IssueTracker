global using Ardalis.GuardClauses;

global using Bogus;

global using IssueTracker.Library.Contracts;
global using IssueTracker.Library.Helpers;
global using IssueTracker.Library.Models;
global using IssueTracker.Library.Services;

global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Options;

global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;
global using MongoDB.Driver;

global using System.Collections.Concurrent;

global using static IssueTracker.Library.Helpers.CollectionNames;
