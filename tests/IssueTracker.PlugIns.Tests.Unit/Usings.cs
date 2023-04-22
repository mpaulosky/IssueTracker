global using System.Diagnostics.CodeAnalysis;

global using FluentAssertions;

global using IssueTracker.CoreBusiness.Models;
global using IssueTracker.PlugIns.Contracts;
global using IssueTracker.PlugIns.DataAccess;
global using IssueTracker.PlugIns.PlugInRepositoryInterfaces;

global using Microsoft.Extensions.Caching.Memory;

global using MongoDB.Driver;

global using Moq;

global using NSubstitute;

global using TestingSupport.Library.Fixtures;

global using Xunit;

global using static IssueTracker.CoreBusiness.Helpers.CollectionNames;
global using static IssueTracker.PlugIns.Tests.Unit.Fixtures.Fixtures;
