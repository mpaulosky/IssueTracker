namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeSolutionTests
{

	public FakeSolutionTests()
	{
	}

	[Theory(DisplayName = "FakeSolution GetNewSolution Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewSolution_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{

		// Arrange
		// Act
		var result = FakeSolution.GetNewSolution(expected);

		// Assert
		switch (expected)
		{
			case true:
				result.Id.Should().NotBeEmpty();
				break;
			default:
				result.Id.Should().BeEmpty();
				break;
		}

		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Issue.Should().NotBeNull();
		result.Author.Should().NotBeNull();
		result.Archived.Should().BeFalse();

	}

	[Fact(DisplayName = "FakeSolution GetNewSolution Test")]
	public void GetNewSolution_With_Empty_Value_Should_Return_WithOut_Id_Test()
	{
		// Arrange

		// Act
		var result = FakeSolution.GetNewSolution();

		// Assert
		result.Id.Should().BeEmpty();
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
