// Copyright (c) 2023. All rights reserved.
// File Name :     BasicStatusModelTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit

namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class BasicStatusModelTests
{
	[Fact(DisplayName = "BasicStatusModel with a StatusModel should make a vaLid BasicStatusModel")]
	public void BasicStatusModel_With_AStatus_Should_BeValid_Test()
	{
		//Arrange
		StatusModel expected = FakeStatus.GetNewStatus(true);

		//Act
		BasicStatusModel result = new(expected);

		//Assert
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);
	}

	[Fact(DisplayName = "BasicStatusModel with StatusName and StatusDescription should make a valid")]
	public void BasicStatusModel_With_AStatusNameAndAStatusDescription_Should_BeValid_Test()
	{
		// Arrange
		StatusModel expected = FakeStatus.GetNewStatus(true);

		// Act
		BasicStatusModel result = new(expected.StatusName, expected.StatusDescription);

		// Assert
		result.StatusName.Should().Be(expected.StatusName);
		result.StatusDescription.Should().Be(expected.StatusDescription);
	}
}