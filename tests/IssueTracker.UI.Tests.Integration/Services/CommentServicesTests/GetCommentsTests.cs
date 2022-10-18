﻿
using System.Collections.Immutable;

namespace IssueTracker.UI.Tests.Integration.Services.CommentServicesTests;

public class GetCommentsTests : IClassFixture<IssueTrackerUIFactory>
{

	private readonly IssueTrackerUIFactory _factory;
	private readonly CommentService _sut;

	public GetCommentsTests(IssueTrackerUIFactory factory)
	{

		_factory = factory;
		var repo = (ICommentRepository)_factory.Services.GetRequiredService(typeof(ICommentRepository));
		var memCache = (IMemoryCache)_factory.Services.GetRequiredService(typeof(IMemoryCache));
		_sut = new CommentService(repo, memCache);

	}

	[Fact]
	public async Task GetComments_With_ValidData_Should_ReturnComments_Test()
	{

		// Arrange
		var expected = FakeComment.GetNewComment();
		await _sut.CreateComment(expected);

		// Act
		var result = await _sut.GetComments();

		// Assert
		result.Count.Should().Be(1);
		result[0].Id.Should().Be(expected.Id);
		result[0].Comment.Should().BeEquivalentTo(expected.Comment);
		result[0].Author.Should().BeEquivalentTo(expected.Author);
		result[0].Issue.Should().BeEquivalentTo(expected.Issue);

	}

}