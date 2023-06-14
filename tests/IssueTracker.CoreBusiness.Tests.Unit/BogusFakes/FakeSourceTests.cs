﻿// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeSourceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

using IssueTracker.CoreBusiness.Models;

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeSourceTests
{
	[Fact(DisplayName = "FakeSource GetSource Test")]
	public void GetSource_With_RequestForFakeSource_Should_Return_AValidBasicCommentSourceModel_Test()
	{
		// Arrange

		// Act
		BasicCommentOnSourceModel result = FakeSource.GetSource();

		// Assert
		result.Id.Should().NotBeNull();
		result.Title.Should().NotBeNull();
		result.Description.Should().NotBeNull();
		result.Author.Should().NotBeNull();
	}
}