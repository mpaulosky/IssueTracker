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

// MongoDB container resource
var mongodb = builder
	.AddContainer("mongodb", "mongo")
	.WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "admin")
	.WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "admin");

// Blazor UI service
var ui = builder
	.AddProject<Projects.IssueTracker_UI>("ui")
	.WaitFor(mongodb);

builder.Build().Run();
