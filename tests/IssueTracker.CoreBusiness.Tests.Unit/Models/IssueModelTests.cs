// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     IssueModelTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit
// =============================================

using MongoDB.Bson;

namespace IssueTracker.CoreBusiness.Models;

[ExcludeFromCodeCoverage]
public class IssueModelTests
{
	[Fact]
	public void Approved_With_True_Should_Be_Stored_As_approved_for_release_Test()
	{
		// Arrange
		IssueModel issue = new() { Id = ObjectId.GenerateNewId().ToString(), Approved = true };

		// Act
		BsonDocument document = issue.ToBsonDocument();

		// Assert
		document["approved_for_release"].AsBoolean.Should().BeTrue();
		document.Contains(nameof(IssueModel.Approved)).Should().BeFalse();
	}
}
