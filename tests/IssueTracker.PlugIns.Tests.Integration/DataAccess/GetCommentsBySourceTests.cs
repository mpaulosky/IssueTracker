﻿namespace IssueTracker.PlugIns.DataAccess;[ExcludeFromCodeCoverage][Collection("Test Collection")]public class GetCommentsBySourceTests : IAsyncLifetime{	private const string? CleanupValue = "comments";	private readonly IssueTrackerTestFactory _factory;	private readonly CommentRepository _sut;	public GetCommentsBySourceTests(IssueTrackerTestFactory factory)	{		_factory = factory;		var context = _factory.Services.GetRequiredService<IMongoDbContextFactory>();		_sut = new CommentRepository(context);	}	public Task InitializeAsync()	{		return Task.CompletedTask;	}	public async Task DisposeAsync()	{		await _factory.ResetCollectionAsync(CleanupValue);	}	[Fact(DisplayName = "GetBySourceAsync With Valid Data Should Succeed")]	public async Task GetBySourceAsync_With_ValidData_Should_ReturnValidComment_Test()	{		// Arrange									var expected = FakeComment.GetNewComment();		await _sut.CreateAsync(expected);		// Act								var result = (await _sut.GetBySourceAsync(expected.CommentOnSource!)).ToList();		// Assert										result.Should().NotBeNull();		result.Should().HaveCount(1);		result[0].CommentOnSource!.Id.Should().Be(expected.CommentOnSource!.Id);	}}
