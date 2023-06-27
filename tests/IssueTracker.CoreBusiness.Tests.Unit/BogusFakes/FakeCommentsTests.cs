// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeCommentsTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCommentsTests
{
	[Theory(DisplayName = "FakeComments GetNewComment Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewComment_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange
		// Act
		CommentModel result = FakeComment.GetNewComment(expected);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }

		result.Should().BeEquivalentTo(FakeComment.GetNewComment(expected),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.Issue!.Id)
				.Excluding(t => t.Issue!.DateCreated)
				.Excluding(t => t.Issue!.Author.Id));
	}

	[Theory(DisplayName = "FakeComment GetComments Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetComments_With_RequestForComments_Should_ReturnFakeComments_Test(int expectedCount)
	{
		// Arrange

		// Act
		List<CommentModel> result = FakeComment.GetComments(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeComment.GetComments(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.ArchivedBy.Id)
				.Excluding(t => t.Issue!.Id)
				.Excluding(t => t.Issue!.DateCreated)
				.Excluding(t => t.Issue!.Author.Id));
	}

	[Theory(DisplayName = "FakeComment GetBasicComments Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicComments_With_RequestForBasicComments_Should_ReturnFakeBasicComments_Test(int expectedCount)
	{
		// Arrange

		// Act
		List<BasicCommentModel> result = FakeComment.GetBasicComments(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeComment.GetBasicComments(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.Issue.Id)
				.Excluding(t => t.Issue.DateCreated)
				.Excluding(t => t.Issue.Author.Id));
	}

	[Theory(DisplayName = "FakeComments GetNewComment With New Seed Test")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewComment_With_Boolean_WithNewSeed_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange
		// Act
		CommentModel result = FakeComment.GetNewComment(expected, true);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }

		result.Should().NotBeEquivalentTo(FakeComment.GetNewComment(expected, true));
	}

	[Theory(DisplayName = "FakeComment GetComments With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetComments_With_RequestForCommentsWithNewSeed_Should_ReturnFakeComments_Test(int expectedCount)
	{
		// Arrange

		// Act
		List<CommentModel> result = FakeComment.GetComments(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeComment.GetComments(expectedCount, true));
	}

	[Theory(DisplayName = "FakeComment GetBasicComments With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicComments_With_RequestForBasicCommentsWithNewSeed_Should_ReturnFakeBasicComments_Test(
		int expectedCount)
	{
		// Arrange

		// Act
		List<BasicCommentModel> result = FakeComment.GetBasicComments(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeComment.GetBasicComments(expectedCount, true));
	}
}