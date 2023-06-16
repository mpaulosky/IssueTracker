// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeSolutionTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeSolutionTests
{
	[Theory(DisplayName = "FakeSolution GetNewSolution Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewSolution_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange
		// Act
		var result = FakeSolution.GetNewSolution(expected);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().BeEquivalentTo(FakeSolution.GetNewSolution(expected),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.Issue.Id)
				.Excluding(t => t.UserVotes)
				.Excluding(t => t.Issue.Author.Id));
	}

	[Theory(DisplayName = "FakeSolution GetSolutions Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetSolutions_With_An_Int_Value_Should_Return_Multiple_Solutions_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeSolution.GetSolutions(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeSolution.GetSolutions(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.Issue.Id)
				.Excluding(t => t.UserVotes)
				.Excluding(t => t.Issue.Author.Id));
	}

	[Theory(DisplayName = "FakeSolution GetBasicSolutions Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicSolutions_With_IntValue_Should_ReturnSameModelsTest(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeSolution.GetBasicSolutions(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeSolution.GetBasicSolutions(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.Issue.Id)
				.Excluding(t => t.Issue.Author.Id));
	}

	[Theory(DisplayName = "FakeSolution GetNewSolution With New Seed Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewSolution_With_BooleanWithNewSeed_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange
		// Act
		var result = FakeSolution.GetNewSolution(expected, true);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().NotBeEquivalentTo(FakeSolution.GetNewSolution(expected, true));
	}

	[Theory(DisplayName = "FakeSolution GetSolutions With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetSolutions_With_An_Int_ValueWithNewSeed_Should_Return_Multiple_Solutions_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeSolution.GetSolutions(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeSolution.GetSolutions(expectedCount, true));
	}

	[Theory(DisplayName = "FakeSolution GetBasicSolutions With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicSolutions_WithNewSeed_Should_ReturnRandomModels_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeSolution.GetBasicSolutions(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeSolution.GetBasicSolutions(expectedCount, true));
	}
}