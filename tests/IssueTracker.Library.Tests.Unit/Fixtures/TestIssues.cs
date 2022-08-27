namespace IssueTracker.Library.Tests.Unit.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestIssues
{
	public static IEnumerable<IssueModel> GetIssues()
	{
		var issues = new List<IssueModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 1",
				Description = "A new test issue 1",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new StatusModel{
					Id = Guid.NewGuid().ToString(),
					StatusName = "Watching",
					StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
				},
				OwnerNotes = "Notes for Issue 1",
				Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
				ApprovedForRelease = true,
				Rejected = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 2",
				Description = "A new test issue 2",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new StatusModel{
					Id = Guid.NewGuid().ToString(),
					StatusName = "Watching",
					StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
				},
				OwnerNotes = "Notes for Issue 2",
				Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
				ApprovedForRelease = false,
				Rejected = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new StatusModel{
					Id = Guid.NewGuid().ToString(),
					StatusName = "Watching",
					StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
				},
				OwnerNotes = "Notes for Issue 3",
				Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
				ApprovedForRelease = true,
				Rejected = false
			}
		};

		return issues;
	}

	public static IEnumerable<IssueModel> GetIssuesWithDuplicateAuthors()
	{
		var issues = new List<IssueModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 1",
				Description = "A new test issue 1",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new StatusModel{
					Id = Guid.NewGuid().ToString(),
					StatusName = "Watching",
					StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
				},
				OwnerNotes = "Notes for Issue 1",
				Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 2",
				Description = "A new test issue 2",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
				IssueStatus = new StatusModel{
					Id = Guid.NewGuid().ToString(),
					StatusName = "Watching",
					StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
				},
				OwnerNotes = "Notes for Issue 2",
				Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
				IssueStatus = new StatusModel{
					Id = Guid.NewGuid().ToString(),
					StatusName = "Watching",
					StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
				},
				OwnerNotes = "Notes for Issue 3",
				Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
			}
		};

		return issues;
	}

	public static IssueModel GetUpdatedIssue()
	{
		var issue = new IssueModel
		{
			Id = "5dc1039a1521eaa36835e542",
			IssueName = "Updated Test Issue 1",
			Description = "Updated A new test issue 1",
			DateCreated = DateTime.UtcNow,
			Archived = false,
			Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
			IssueStatus = new StatusModel{
				Id = Guid.NewGuid().ToString(),
				StatusName = "Watching",
				StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
			},
			OwnerNotes = "Notes for Issue 1",
			Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
		};

		return issue;
	}

	public static IssueModel GetKnownIssue()
	{
		var issue = new IssueModel
		{
			Id = "5dc1039a1521eaa36835e542",
			IssueName = "Test Issue 1",
			Description = "A new test issue 1",
			DateCreated = DateTime.UtcNow,
			Archived = false,
			Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
			IssueStatus = new StatusModel{
				Id = Guid.NewGuid().ToString(),
				StatusName = "Watching",
				StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
			},
			OwnerNotes = "Notes for Issue 1",
			Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
		};

		return issue;
	}

	public static IssueModel GetNewIssue()
	{
		var issue = new IssueModel
		{
			IssueName = "Test Issue 1",
			Description = "A new test issue",
			DateCreated = DateTime.UtcNow,
			Archived = false,
			Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
			IssueStatus = new StatusModel(),
			OwnerNotes = "Notes for Issue 1",
			Category = new CategoryModel{ Id = Guid.NewGuid().ToString(), CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits." },
		};

		return issue;
	}

	public static IssueModel GetIssue(
		string id,
		string issueName,
		string description,
		DateTime dateCreated,
		bool archived,
		StatusModel status,
		string ownerNotes)
	{
		var issue = new IssueModel
		{
			Id = id,
			IssueName = issueName,
			Description = description,
			DateCreated = dateCreated,
			Archived = archived,
			IssueStatus = status,
			OwnerNotes = ownerNotes
		};

		return issue;
	}
}