namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeCommentsTests
{

	public FakeCommentsTests()
	{
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
		result.First().CommentOnSource.Should().NotBeNull();
		result.First().Title.Should().NotBeNull();
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

	}

}
