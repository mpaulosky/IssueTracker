﻿namespace IssueTracker.Library.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCommentsTests
{

	[Fact]
	public void GetComments_With_RequestForComments_Should_ReturnFakeComments_Test()
	{

		// Arrange


		// Act
		var result = FakeComment.GetComments(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Source.Should().NotBeNull();
		result.First().Comment.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();

	}

	[Fact]
	public void GetBasicComments_With_RequestForBasicComments_Should_ReturnFakeBasicComments_Test()
	{

		// Arrange

		// Act
		var result = FakeComment.GetBasicComments(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Comment.Should().NotBeNull();

	}

	[Fact]
	public void GetBasicComment_With_RequestForABasicComment_Should_ReturnAFakeBasicComment_Test()
	{

		// Arrange

		// Act
		BasicCommentModel result = FakeComment.GetBasicComment();

		// Assert
		result.Id.Should().NotBeNull();
		result.Comment.Should().NotBeNull();

	}

}