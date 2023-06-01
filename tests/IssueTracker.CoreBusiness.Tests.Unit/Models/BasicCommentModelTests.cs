namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class BasicCommentModelTests
{
	[Fact(DisplayName = "BasicCommentModel With Comment Test")]
	public void BasicCommentModel_With_Comment_Should_Return_A_BasicComment_Test()
	{
		// Arrange
		var expected = FakeComment.GetNewComment(true);

		// Act
		var result = new BasicCommentModel(expected);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expected.Id);
		result.Title.Should().Be(expected.Title);
		result.Description.Should().Be(expected.Description);
		result.DateCreated.Should().Be(expected.DateCreated);
		result.Author.Should().Be(expected.Author);
	}
}
