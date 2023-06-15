// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeStatusTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeStatusTests
{
	[Theory(DisplayName = "FakeStatus GetNewStatus Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewStatus_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange

		// Act
		var result = FakeStatus.GetNewStatus(expected);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().BeEquivalentTo(FakeStatus.GetNewStatus(expected),
			options => options.Excluding(t => t.Id));

	}

	[Fact(DisplayName = "FakeStatus GetStatuses List Test")]
	public void GetStatuses_With_No_Variable_Should_Return_A_List_Of_Statuses_Test()
	{
		// Arrange
		const int expectedCount = 4;

		// Act
		var result = FakeStatus.GetStatuses();

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeStatus.GetStatuses(),
			options => options.Excluding(t => t.Id));
	}

	[Theory(DisplayName = "FakeStatus GetStatuses With Number Needed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetStatuses_With_NumberNeeded_Should_ReturnStatuses_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeStatus.GetStatuses(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeStatus.GetStatuses(expectedCount),
			options => options.Excluding(t => t.Id));
	}


	[Theory(DisplayName = "FakeStatus GetBasicStatuses Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicStatuses_With_RequestForBasicStatuses_Should_ReturnFakeBasicStatuses_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeStatus.GetBasicStatuses(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeStatus.GetBasicStatuses(expectedCount));
	}

	[Theory(DisplayName = "FakeStatus GetNewStatus With New Seed Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewStatus_With_BooleanAndWithNewSeed_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange

		// Act
		var result = FakeStatus.GetNewStatus(expected, true);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().NotBeEquivalentTo(FakeStatus.GetNewStatus(expected, true),
			options => options.Excluding(t => t.Id));

	}

	[Theory(DisplayName = "FakeStatus GetStatuses With Number Needed With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetStatuses_With_NumberNeededAndWithNewSeed_Should_ReturnStatuses_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeStatus.GetBasicStatuses(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeStatus.GetBasicStatuses(expectedCount, true));
	}


	[Theory(DisplayName = "FakeStatus GetBasicStatuses With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicStatuses_With_RequestForBasicStatusesWithNewSeed_Should_ReturnFakeBasicStatuses_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeStatus.GetBasicStatuses(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeStatus.GetBasicStatuses(expectedCount, true));
	}
}