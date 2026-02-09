// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     GetCommentsByIssueTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsByIssueTests : IAsyncLifetime
{
	private const string CleanupValue = "comments";
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;

	public GetCommentsByIssueTests(IssueTrackerTestFactory factory)
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
		await _factory.ResetDatabaseAsync();
	}

	[Fact(DisplayName = "GetByIssueAsync With Valid Data Should Succeed")]
	public async Task GetByIssueAsync_With_ValidData_Should_ReturnValidComment_Test()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		List<CommentModel> result = (await _sut.GetByIssueAsync(expected.Issue!)).ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].Issue!.Id.Should().Be(expected.Issue!.Id);
	}
}