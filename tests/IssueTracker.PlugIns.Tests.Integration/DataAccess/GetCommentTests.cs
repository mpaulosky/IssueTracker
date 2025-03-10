﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     GetCommentTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentTests : IAsyncLifetime
{
	private const string CleanupValue = "comments";

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;

	public GetCommentTests(IssueTrackerTestFactory factory)
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
	public async Task GetComment_With_WithData_Should_ReturnAValidComment_TestAsync()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		CommentModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Id.Should().Be(expected.Id);
		result.Title.Should().BeEquivalentTo(expected.Title);
		result.Author.Should().BeEquivalentTo(expected.Author);
	}

	[Fact]
	public async Task GetComment_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		const string id = "62cf2ad6326e99d665759e5a";

		// Act
		CommentModel result = await _sut.GetAsync(id);

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public async Task GetComment_With_Empty_Id_Should_ThrowArgumentException_Test()
	{
		// Arrange
		
		const string id = "";

		// Act
		
		Func<Task<CommentModel>> act = async () => await _sut.GetAsync(id);

		// Assert
		
		await act.Should().ThrowAsync<ArgumentException>();
	}
}