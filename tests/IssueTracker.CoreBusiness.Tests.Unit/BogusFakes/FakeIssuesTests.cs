namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeIssuesTests
{

	public FakeIssuesTests()
	{
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
