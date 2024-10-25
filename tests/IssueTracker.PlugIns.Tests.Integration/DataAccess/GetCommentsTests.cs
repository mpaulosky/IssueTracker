﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     GetCommentsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsTests : IAsyncLifetime
{
	private const string CleanupValue = "comments";

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;

	public GetCommentsTests(IssueTrackerTestFactory factory)
	{
		_factory = factory;
		IMongoDbContextFactory context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	public async Task DisposeAsync()
	{
		await _factory.ResetCollectionAsync(CleanupValue);
	}

	[Fact]
	public async Task GetComments_With_ValidData_Should_ReturnComments_Test()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		List<CommentModel> results = (await _sut.GetAllAsync())!.ToList();

		// Assert
		results.Count.Should().Be(1);
		results[0].Title.Should().Be(expected.Title);
		results[0].Author.Should().BeEquivalentTo(expected.Author);
	}
}