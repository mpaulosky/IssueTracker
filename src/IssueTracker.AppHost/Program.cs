// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.AppHost
// =============================================

namespace IssueTracker.AppHost;

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB container resource
var mongodb = builder
	.AddContainer("mongodb", "mongo")
	.WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "admin")
	.WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "admin")
	.WithBindPort(27017, 27017);

// Blazor UI service
var ui = builder
	.AddProject<Projects.IssueTracker_UI>("ui")
	.WithReference(mongodb)
	.WaitFor(mongodb);

builder.Build().Run();
