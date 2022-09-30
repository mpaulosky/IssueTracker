namespace IssueTracker.Library.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeIssuesTests
{

	[Fact()]
	public void GetIssues_With_RequestForIssues_Should_ReturnFakeIssues_Test()
	{

		// Arrange

		// Act
		var result = FakeIssue.GetIssues(1);

		// Assert
		result.Count().Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().IssueName.Should().NotBeNull();
		result.First().Description.Should().NotBeNull();
		result.First().Category.Should().NotBeNull();
		result.First().IssueStatus.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();

	}

	[Fact()]
	public void GetBasicIssues_With_RequestForBasicIssues_Should_ReturnFakeBasicIssues_Test()
	{
		// Arrange

		// Act
		var result = FakeIssue.GetBasicIssues(1);

		// Assert
		result.Count().Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().Issue.Should().NotBeNull();
		result.First().Description.Should().NotBeNull();
		result.First().Category.Should().NotBeNull();
		result.First().Status.Should().NotBeNull();
		result.First().Author.Should().NotBeNull();

	}

}