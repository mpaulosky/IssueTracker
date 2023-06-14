// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     UpdateCommentTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.PlugIns.Tests.Integration
// =============================================

namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCommentTests : IAsyncLifetime
{
	private const string CleanupValue = "comments";

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;

	public UpdateCommentTests(IssueTrackerTestFactory factory)
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

	[Fact(DisplayName = "UpdateAsync With Valid Data Should Succeed")]
	public async Task UpdateAsync_With_ValidData_Should_UpdateTheComment_Test()
	{
		// Arrange
		CommentModel expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		expected.Title = "Updated";
		await _sut.UpdateAsync(expected.Id, expected);
		CommentModel result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Author.Should().BeEquivalentTo(expected.Author);
		result.CommentOnSource!.SourceType.Should().Be(expected.CommentOnSource!.SourceType);
	}

	[Fact(DisplayName = "UpdateAsync With Invalid Data Should Fail")]
	public async Task UpdateAsync_With_WithInValidData_Should_ThrowArgumentNullException_Test()
	{
		// Arrange

		// Act
		Func<Task> act = async () => await _sut.UpdateAsync(null!, null!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();
	}
}