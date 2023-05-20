﻿namespace IssueTracker.PlugIns.DataAccess;

[ExcludeFromCodeCoverage]
[Collection("Test Collection")]
public class GetCommentTests : IAsyncLifetime
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentRepository _sut;
	private const string CleanupValue = "comments";

	public GetCommentTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;
		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();
		_sut = new CommentRepository(context);

	}

	[Fact]
	public async Task GetComment_With_WithData_Should_ReturnAValidComment_TestAsync()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateAsync(expected);

		// Act
		var result = await _sut.GetAsync(expected.Id);

		// Assert
		result.Id.Should().Be(expected.Id);
		result.Title.Should().BeEquivalentTo(expected.Title);
		result.Author.Should().BeEquivalentTo(expected.Author);
		result.CommentOnSource!.SourceType.Should().Be(expected.CommentOnSource!.SourceType);

	}

	[Fact]
	public async Task GetComment_With_WithoutData_Should_ReturnNothing_TestAsync()
	{
		// Arrange
		const string id = "62cf2ad6326e99d665759e5a";

		// Act
		var result = await _sut.GetAsync(id);

		// Assert
		result.Should().BeNull();

	}

	[Fact]
	public async Task GetComment_With_NullId_Should_ThrowArgumentNullException_Test()
	{

		// Arrange

		// Act
		Func<Task<CommentModel>> act = async () => await _sut.GetAsync(null!);

		// Assert
		await act.Should().ThrowAsync<ArgumentNullException>();

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
