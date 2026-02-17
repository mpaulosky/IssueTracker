// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

using Aspire.Hosting;
using IssueTracker.AppHost.Extensions;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB container resource with Aspire API
var mongodb = builder.AddMongoDB("mongodb")
	.WithDataVolume()
	.WithHealthCheck("mongodb");

// Redis cache resource using extension method
var redis = builder.AddRedisCache();

// Blazor UI service
var ui = builder
	.AddProject<Projects.IssueTracker_UI>("ui")
	.WithReference(mongodb)
	.WithReference(redis);

var app = builder.Build();

await app.RunAsync();
