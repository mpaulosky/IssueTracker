// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeUserTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeUserTests
{
	[Theory(DisplayName = "FakeUser GetNewUser Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewUser_With_Boolean_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange

		// Act
		UserModel result = FakeUser.GetNewUser(expected);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().BeEquivalentTo(FakeUser.GetNewUser(expected),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.ObjectIdentifier));
	}

	[Theory(DisplayName = "FakeUser GetUsers Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetUser_WhenUserRequested_Returns_FakeUser_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeUser.GetUsers(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeUser.GetUsers(expectedCount),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.ObjectIdentifier));
	}

	[Theory(DisplayName = "FakeUser GetBasicUsers Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicUser_WhenBasicUserRequested_Returns_FakeBasicUser_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeUser.GetBasicUser(expectedCount);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().BeEquivalentTo(FakeUser.GetBasicUser(expectedCount),
			options => options
				.Excluding(t => t.Id));
	}

	[Theory(DisplayName = "FakeUser GetNewUser With New Seed Tests")]
	[InlineData(true)]
	[InlineData(false)]
	public void GetNewUser_With_BooleanAndWithNewSeed_Should_Return_With_Or_Without_An_Id_Test(bool expected)
	{
		// Arrange

		// Act
		var result = FakeUser.GetNewUser(expected, true);

		// Assert
		if (!expected) { result.Id.Should().BeNullOrWhiteSpace(); }
		result.Should().NotBeEquivalentTo(FakeUser.GetNewUser(expected, true));
	}

	[Theory(DisplayName = "FakeUser GetUsers With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetUser_With_NumberRequestedAndWithNewSeed_Returns_FakeUser_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeUser.GetUsers(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeUser.GetUsers(expectedCount, true));
	}

	[Theory(DisplayName = "FakeUser GetBasicUsers With New Seed Test")]
	[InlineData(1)]
	[InlineData(5)]
	public void GetBasicUser_With_NumberRequestedAndWithNewSeed_Returns_FakeBasicUser_Test(int expectedCount)
	{
		// Arrange

		// Act
		var result = FakeUser.GetBasicUser(expectedCount, true);

		// Assert
		result.Count.Should().Be(expectedCount);
		result.Should().NotBeEquivalentTo(FakeUser.GetBasicUser(expectedCount, true));
	}
}