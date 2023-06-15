// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeIssuesTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

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
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().BeEquivalentTo(FakeIssue.GetNewIssue(expected),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id));

	}

	[Theory(DisplayName = "FakeIssue GetIssues Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetIssues_With_RequestForIssues_Should_ReturnFakeIssues_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeIssue.GetIssues(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeIssue.GetIssues(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author.Id)
				.Excluding(t => t.ArchivedBy.Id));
	}

	[Theory(DisplayName = "FakeIssue GetBasicIssues Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicIssues_With_RequestForBasicIssues_Should_ReturnFakeBasicIssues_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeIssue.GetBasicIssues(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeIssue.GetBasicIssues(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.Author.Id));
	}

	[Theory(DisplayName = "FakeIssue GetNewIssue With New Seed Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewIssue_With_BooleanWithNewSeed_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange

		// Act
		var result = FakeIssue.GetNewIssue(expected, true);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().NotBeEquivalentTo(FakeIssue.GetNewIssue(expected, true));

	}

	[Theory(DisplayName = "FakeIssue GetIssues With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetIssues_With_RequestForIssuesWithNewSeed_Should_ReturnFakeIssues_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeIssue.GetIssues(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeIssue.GetIssues(expectedCount, true));
	}

	[Theory(DisplayName = "FakeIssue GetBasicIssues With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicIssues_With_RequestForBasicIssuesWithNewSeed_Should_ReturnFakeBasicIssues_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeIssue.GetBasicIssues(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeIssue.GetBasicIssues(expectedCount, true));
	}
}