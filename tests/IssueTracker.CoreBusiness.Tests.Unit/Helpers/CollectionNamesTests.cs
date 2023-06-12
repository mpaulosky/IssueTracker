// Copyright (c) 2023. All rights reserved.
// File Name :     CollectionNamesTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : IssueTracker
// Project Name :  IssueTracker.CoreBusiness.Tests.Unit

namespace IssueTracker.CoreBusiness.Helpers;

[ExcludeFromCodeCoverage]
public class CollectionNamesTests
{
	[Theory(DisplayName = "GetCollectionName Tests")]
	[InlineData("CategoryModel", "categories")]
	[InlineData("CommentModel", "comments")]
	[InlineData("IssueModel", "issues")]
	[InlineData("SolutionModel", "solutions")]
	[InlineData("StatusModel", "statuses")]
	[InlineData("UserModel", "users")]
	public void GetCollectionName_WithValidInput_Should_ReturnExpectedValue(string entityName, string expected)
	{
		// Arrange

		// Act
		string result = CollectionNames.GetCollectionName(entityName);

		// Assert
		result.Should().Be(expected);
	}
}
