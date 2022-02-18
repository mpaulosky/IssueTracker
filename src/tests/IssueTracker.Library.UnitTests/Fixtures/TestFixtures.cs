using IssueTrackerLibrary.Models;

using MongoDB.Bson;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IssueTrackerLibraryUnitTests.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestFixtures
{
	public static IEnumerable<User> GetUsers()
	{
		var expected = new List<User>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Jim",
				LastName = "Test",
				DisplayName = "jimtest",
				EmailAddress = "jim.test@test.com",
				AuthoredIssues = new List<BasicIssueModel>(),
				VotedOnComments = new List<BasicCommentModel>(),
				AuthoredComments = new List<BasicCommentModel>()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Sam",
				LastName = "Test",
				DisplayName = "samtest",
				EmailAddress = "sam.test@test.com",
				AuthoredIssues = new List<BasicIssueModel>(),
				VotedOnComments = new List<BasicCommentModel>(),
				AuthoredComments = new List<BasicCommentModel>()
			},
			new ()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Tim",
				LastName = "Test",
				DisplayName = "timtest",
				EmailAddress = "tim.test@test.com",
				AuthoredIssues = new List<BasicIssueModel>(),
				VotedOnComments = new List<BasicCommentModel>(),
				AuthoredComments = new List<BasicCommentModel>()
			},
		};
		
		return expected;
	}

	public static User GetUser(string userId, string objectIdentifier, string firstName, string lastName, string displayName, string email)
	{
		var expected = new User()
		{
			Id = userId,
			ObjectIdentifier = objectIdentifier,
			FirstName = firstName,
			LastName = lastName,
			DisplayName = "jimtest",
			EmailAddress = "jim.test@test.com",
			AuthoredIssues = new List<BasicIssueModel>(),
			VotedOnComments = new List<BasicCommentModel>(),
			AuthoredComments = new List<BasicCommentModel>()
		};

		return expected;
	}
}