﻿namespace IssueTracker.Library.Services.CommentServicesTests;

[ExcludeFromCodeCoverage]
[Collection("Database")]
public class GetCommentsTests : IClassFixture<IssueTrackerTestFactory>
{

	private readonly IssueTrackerTestFactory _factory;
	private readonly CommentService _sut;

	public GetCommentsTests(IssueTrackerTestFactory factory)
	{

		_factory = factory;

		var db = (IMongoDbContextFactory)_factory.Services.GetRequiredService(typeof(IMongoDbContextFactory));
		db.Database.DropCollection(CollectionNames.GetCollectionName(nameof(CommentModel)));

		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		memCache.Remove("CommentsData");


		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task GetComments_With_ValidData_Should_ReturnComments_Test()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		var results = await _sut.GetComments();

		// Assert
		results.Count.Should().Be(1);
		results[0].Comment.Should().Be(expected.Comment);
		results[0].Author.Should().BeEquivalentTo(expected.Author);
		results[0].Issue.Should().BeEquivalentTo(expected.Issue);

	}

}