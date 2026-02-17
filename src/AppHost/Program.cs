// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  AppHost
// =============================================

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB container resource with Aspire API
var mongodb = builder.AddMongoDB("mongodb")
	.WithDataVolume()
	.WithHealthCheck("mongodb");

// Redis container resource with Aspire API
var redis = builder.AddRedis("redis")
	.WithDataVolume()
	.WithHealthCheck("redis");

// Blazor UI service
var ui = builder
	.AddProject<Projects.IssueTracker_UI>("ui")
	.WithReference(mongodb)
	.WithReference(redis);

var app = builder.Build();

await app.RunAsync();
