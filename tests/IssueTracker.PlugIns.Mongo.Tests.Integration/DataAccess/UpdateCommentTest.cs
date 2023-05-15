﻿namespace IssueTracker.PlugIns.Mongo.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class UpdateCommentTest : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string? CleanupValue = "comments";

	public UpdateCommentTest(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact(DisplayName = "UpdateAsync With Valid Data Should Update Successfully")]
	public async Task UpdateAsync_With_Valid_Data_Should_Update_Successfully_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();

		await _sut.CreateAsync(expected).ConfigureAwait(false);

		var update = new CommentModel
		{
			Id = expected.Id,
			Title = expected.Title,
			Description = "Updated",
			Archived = expected.Archived
		};

		// Act
		await _sut.UpdateAsync(update).ConfigureAwait(false);

		var result = await _sut.GetAsync(expected.Id).ConfigureAwait(false);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(expected.Id);
		result.Description.Should().Be(update.Description);

	}

	public Task InitializeAsync()
	{

		return Task.CompletedTask;

	}

	public async Task DisposeAsync()
	{

		await _factory.ResetCollectionAsync(CleanupValue);

	}

}
