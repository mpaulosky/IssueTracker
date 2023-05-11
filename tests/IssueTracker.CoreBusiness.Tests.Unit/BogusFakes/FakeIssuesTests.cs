namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeIssuesTests
{

	[Theory(DisplayName = "FakeIssue GetNewIssue Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewIssue_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{

		// Arrange
		// Act
		var result = FakeIssue.GetNewIssue(expected);

		// Assert
		switch (expected)
		{
			case true:
				result.Id.Should().NotBeNull();
				break;
			default:
				result.Id.Should().BeNullOrEmpty();
				break;
		}

		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Category.Should().NotBeNull();
		result.IssueStatus.Should().NotBeNull();
		result.Author.Should().NotBeNull();
		result.Archived.Should().BeFalse();

	}

	[Fact(DisplayName = "FakeIssue GetIssues Test")]
	public void GetIssues_With_RequestForIssues_Should_ReturnFakeIssues_Test()
	{

		// Arrange

		// Act
		var result = FakeIssue.GetIssues(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Title.Should().NotBeNull();
		result.First().Description.Should().NotBeNull();
		result.First().Category.Should().NotBeNull();
		result.First().IssueStatus.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();

	}

	[Fact(DisplayName = "FakeIssue GetBasicIssues Test")]
	public void GetBasicIssues_With_RequestForBasicIssues_Should_ReturnFakeBasicIssues_Test()
	{
		// Arrange

		// Act
		var result = FakeIssue.GetBasicIssues(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Title.Should().NotBeNull();
		result.First().Description.Should().NotBeNull();
		result.First().Category.Should().NotBeNull();
		result.First().Status.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();

	}

}
