﻿namespace IssueTracker.CoreBusiness.BogusFakes;

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
		var result = FakeComment.GetNewComment(expected);

		// Assert
		switch (expected)
		{
			case true:
				result.Id.Should().NotBeNull();
				break;
			default:
				result.Id.Should().BeNullOrWhiteSpace();
				break;
		}
		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.CommentOnSource.Should().NotBeNull();
		result.Author.Should().NotBeNull();
		result.Archived.Should().BeFalse();

	}
	[Fact(DisplayName = "FakeComment GetComments Test")]
	public void GetComments_With_RequestForComments_Should_ReturnFakeComments_Test()
	{

		// Arrange

		// Act
		var result = FakeComment.GetComments(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Title.Should().NotBeNull();
		result.First().Description.Should().NotBeNull();
		result.First().CommentOnSource.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeComment GetBasicComments Test")]
	public void GetBasicComments_With_RequestForBasicComments_Should_ReturnFakeBasicComments_Test()
	{

		// Arrange

		// Act
		var result = FakeComment.GetBasicComments(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Title.Should().NotBeNull();
		result.First().Description.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();
		result.First().CommentOnSource.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeComment GetBasicComment Test")]
	public void GetBasicComment_With_RequestForABasicComment_Should_ReturnAFakeBasicComment_Test()
	{

		// Arrange

		// Act
		BasicCommentModel result = FakeComment.GetBasicComments(1).First();

		// Assert
		result.Id.Should().NotBeNull();
		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Author.Should().NotBeNull();
		result.CommentOnSource.Should().NotBeNull();

	}

}
