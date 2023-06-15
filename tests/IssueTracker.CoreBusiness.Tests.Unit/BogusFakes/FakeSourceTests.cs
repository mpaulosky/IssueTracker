// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     FakeSourceTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

namespace IssueTracker.CoreBusiness.BogusFakes;

[ExcludeFromCodeCoverage]
public class FakeSourceTests
{
	[Fact(DisplayName = "FakeSource GetSource Test")]
	public void GetSource_With_RequestForFakeSource_Should_Return_AValidBasicCommentSourceModel_Test()
	{
		// Arrange

		// Act
		var result = FakeSource.GetSource();

		// Assert
		result.Should().BeEquivalentTo(FakeSource.GetSource(),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author!.Id));
	}

	[Fact(DisplayName = "FakeSource GetSource With New Seed Test")]
	public void GetSource_With_RequestForFakeSourceWithNewSeed_Should_Return_AValidBasicCommentSourceModel_Test()
	{
		// Arrange

		// Act
		var result = FakeSource.GetSource(true);

		// Assert
		result.Should().NotBeEquivalentTo(FakeSource.GetSource(true),
			options => options
				.Excluding(t => t.Id)
				.Excluding(t => t.DateCreated)
				.Excluding(t => t.Author!.Id));
	}

}