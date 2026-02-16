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

// Blazor UI service
var ui = builder
	.AddProject<Projects.IssueTracker_UI>("ui")
	.WithReference(mongodb);

var app = builder.Build();

await app.RunAsync();
