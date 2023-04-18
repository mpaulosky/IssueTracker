namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeSolutionTests
{

	public FakeSolutionTests()
	{
	}

	[Fact(DisplayName = "FakeSolution GetNewSolution Test")]
	public void GetNewSolution_StateUnderTest_ExpectedBehavior()
	{
		// Arrange

		// Act
		var result = FakeSolution.GetNewSolution();

		// Assert
		result.Id.Should().NotBeNull();
		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Issue.Should().NotBeNull();
		result.Author.Should().NotBeNull();
		result.Archived.Should().BeFalse();

	}

	[Fact(DisplayName = "FakeSolution GetSolutions Test")]
	public void GetSolutions_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		const int numberOfSolutions = 1;

		// Act
		var result = FakeSolution.GetSolutions(numberOfSolutions).First();

		// Assert
		result.Id.Should().NotBeNull();
		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Issue.Should().NotBeNull();
		result.Author.Should().NotBeNull();
		result.Archived.Should().BeTrue();
	}

	[Fact(DisplayName = "FakeSolution GetBasicSolutions Test")]
	public void GetBasicSolutions_StateUnderTest_ExpectedBehavior()
	{
		// Arrange
		const int numberOfSolutions = 1;

		// Act
		var result = FakeSolution.GetBasicSolutions(numberOfSolutions).First();

		// Assert
		result.Id.Should().NotBeNull();
		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Issue.Should().NotBeNull();
		result.Author.Should().NotBeNull();
	}
}
