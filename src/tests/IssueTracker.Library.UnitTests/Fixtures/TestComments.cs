using IssueTrackerLibrary.Models;

using System;
using System.Collections.Generic;

namespace IssueTracker.Library.UnitTests.Fixtures;

public static class TestComments
{
	public static Comment GetKnownComment()
	{
		var comment = new Comment()
		{
			Id = "5dc1039a1521eaa36835e541",
			CommentName = "Test Comment",
			Archived = false,
			Author = new BasicUserModel() { Id = "5dc1039a1521eaa36835e542", DisplayName = "Test User" },
			DateCreated = DateTime.UtcNow,
			UserVotes = new HashSet<string>(),
			Status = new Status()
		};

		return comment;
	}

	public static IEnumerable<Comment> GetComments()
	{
		var comments = new List<Comment>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				CommentName = "Test Comment 1",
				Archived = false,
				Author = new BasicUserModel() { Id = "5dc1039a1521eaa36835e543", DisplayName = "Test User" },
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string>(),
				Status = new Status()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				CommentName = "Test Comment 2",
				Archived = false,
				Author = new BasicUserModel() { Id = "5dc1039a1521eaa36835e542", DisplayName = "Test User" },
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string>(),
				Status = new Status()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				CommentName = "Test Comment 3",
				Archived = false,
				Author = new BasicUserModel() { Id = "5dc1039a1521eaa36835e542", DisplayName = "Test User" },
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string>(),
				Status = new Status()
			}
		};

		return comments;
	}

	public static Comment GetComment(string id, string commentName, bool archived)
	{
		var comment = new Comment()
		{
			Id = id,
			CommentName = commentName,
			Archived = archived,
		};

		return comment;
	}
}