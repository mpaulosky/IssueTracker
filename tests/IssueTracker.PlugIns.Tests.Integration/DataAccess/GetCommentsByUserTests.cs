// Copyright (c) 2023. All rights reserved.
// File Name :     GetCommentsByUserTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentsByUserTests : IAsyncLifetime
{
	private const string CleanupValue = "comments";
	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;

	public GetCommentsByUserTests(IssueTrackerTestFactory factory)
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

	[Fact(DisplayName = "GetByUserAsync With Valid Data Should Succeed")]
	public async Task GetByUserAsync_With_ValidData_Should_ReturnValidComment_Test()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		List<CommentModel> result = (await _sut.GetByUserAsync(expected.Author.Id)).ToList();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].Author.Id.Should().Be(expected.Author.Id);
	}
}