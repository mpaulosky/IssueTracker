﻿
namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class BasicCommentOnSourceModelTests
{

	[Fact(DisplayName = "BasicCommentOnSourceModel With Comment Test")]
	public void BasicCommentOnSourceModel_With_Comment_Should_Return_A_Valid_Model_Test()
	{

		// Arrange
		var comment = FakeComment.GetNewComment(true);

		// Act
		var result = new BasicCommentOnSourceModel(comment);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(comment.Id);
		result.SourceType.Should().Be("Comment");
		result.Title.Should().Be(comment.Title);
		result.Description.Should().Be(comment.Description);
		result.DateCreated.Should().Be(comment.DateCreated);
		result.Author.Should().Be(comment.Author);

	}

	[Fact(DisplayName = "BasicCommentOnSourceModel With Issue Test")]
	public void BasicCommentOnSourceModel_With_Issue_Should_Return_A_Valid_Model_Test()
	{
		// Arrange
		var issue = FakeIssue.GetNewIssue(true);

		// Act
		var result = new BasicCommentOnSourceModel(issue);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(issue.Id);
		result.SourceType.Should().Be("Issue");
		result.Title.Should().Be(issue.Title);
		result.Description.Should().Be(issue.Description);
		result.DateCreated.Should().Be(issue.DateCreated);
		result.Author.Should().Be(issue.Author);
	}

	[Fact(DisplayName = "BasicCommentOnSourceModel With Null Solution Test")]
	public void BasicCommentOnSourceModel_With_Solution_Should_Return_A_Valid_Model_Test()
	{

		// Arrange
		var solution = FakeSolution.GetNewSolution(true);

		// Act
		var result = new BasicCommentOnSourceModel(solution);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(solution.Id);
		result.SourceType.Should().Be("Solution");
		result.Title.Should().Be(solution.Title);
		result.Description.Should().Be(solution.Description);
		result.DateCreated.Should().Be(solution.DateCreated);
		result.Author.Should().Be(solution.Author);

	}

}
