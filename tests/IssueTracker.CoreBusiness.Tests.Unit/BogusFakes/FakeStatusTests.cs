﻿// Copyright (c) 2023. All rights reserved.
// File Name :     FakeStatusTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit

using IssueTracker.CoreBusiness.Models;

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
		StatusModel result = FakeStatus.GetNewStatus(expected);

		// Assert
		switch (expected)
		{
			case true:
				result.Id.Should().NotBeNull();
				break;
			default:
				result.Id.Should().BeNullOrWhiteSpace();
				break;
		}

		result.StatusName.Should().NotBeNull();
		result.StatusDescription.Should().NotBeNull();
		result.Archived.Should().BeFalse();
	}

	[Fact(DisplayName = "FakeStatus GetStatuses Existing Test")]
	public void GetStatuses_With_No_Variable_Should_Return_A_List_Of_Statuses_Test()
	{
		// Arrange
		const int expected = 4;

		// Act
		List<StatusModel> result = FakeStatus.GetStatuses().ToList();

		// Assert
		result.Count.Should().Be(expected);
		result.First().Id.Should().NotBeNull();
		result.First().StatusName.Should().Be("Answered");
		result.First().StatusDescription.Should().Be("The issue was accepted and the corresponding item was created.");
		result.First().Archived.Should().BeFalse();
	}

	[Fact(DisplayName = "FakeStatus GetStatuses Test")]
	public void GetStatuses_With_RequestForStatuses_Should_ReturnFakeStatuses_Test()
	{
		// Arrange

		// Act
		List<StatusModel> result = FakeStatus.GetStatuses(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().Id.Should().NotBeNull();
		result.First().StatusName.Should().NotBeNull();
		result.First().StatusDescription.Should().NotBeNull();
	}

	[Fact(DisplayName = "FakeStatus GetBasicStatuses Test")]
	public void GetBasicStatuses_With_RequestForBasicStatuses_Should_ReturnFakeBasicStatuses_Test()
	{
		// Arrange

		// Act
		List<BasicStatusModel> result = FakeStatus.GetBasicStatuses(1).ToList();

		// Assert
		result.Count.Should().Be(1);
		result.First().StatusName.Should().NotBeNull();
		result.First().StatusDescription.Should().NotBeNull();
	}
}