namespace IssueTracker.Library.Fixtures;

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
				IssueStatus = new BasicStatusModel
					{
						StatusName = "Watching",
						StatusDescription =
							"The suggestion is interesting. We are watching to see how much interest there is in it."
					},
				OwnerNotes = "Notes for Issue 1",
				Category = new BasicCategoryModel
				{
					CategoryName = "Design", 
					CategoryDescription = "An Issue with the design."
				},
				ApprovedForRelease = false,
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
				IssueStatus = new BasicStatusModel
					{
						StatusName = "Answered",
						StatusDescription =
							"The issue was answered and the answer was accepted."
					},
				OwnerNotes = "Notes for Issue 2",
				Category = new BasicCategoryModel
				{
					CategoryName = "Documentation", 
					CategoryDescription = "An Issue with the documentation."
				},
				ApprovedForRelease = true,
				Rejected = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = true,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new BasicStatusModel
					{
						StatusName = "In Work",
						StatusDescription =
							"An answer to this issue has been submitted."
					},
				OwnerNotes = "Notes for Issue 3",
				Category = new BasicCategoryModel
				{
					CategoryName = "Implementation", CategoryDescription = "An Issue with the implementation."
				},
				ApprovedForRelease = true,
				Rejected = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e545", DisplayName = "jim test" },
				IssueStatus = new BasicStatusModel
					{
						StatusName = "Dismissed",
						StatusDescription =
							"The suggestion was not something that we are going to undertake."
					},
				OwnerNotes = "Notes for Issue 3",
				Category = new BasicCategoryModel
				{
					CategoryName = "Clarification", CategoryDescription = "A quick Issue with a general question."
				},
				ApprovedForRelease = true,
				Rejected = true
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new BasicStatusModel
					{
						StatusName = "Watching",
						StatusDescription =
							"The suggestion is interesting. We are watching to see how much interest there is in it."
					},
				OwnerNotes = "Notes for Issue 3",
				Category = new BasicCategoryModel
				{
					CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
				},
				ApprovedForRelease = false,
				Rejected = false
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 6",
				Description = "A new test issue 6",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new(),
				OwnerNotes = "Notes for Issue 1",
				Category = new BasicCategoryModel
				{
					CategoryName = "Design", 
					CategoryDescription = "An Issue with the design."
				},
				ApprovedForRelease = false,
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
				IssueStatus =
					new BasicStatusModel
					{
						StatusName = "Watching",
						StatusDescription =
							"The suggestion is interesting. We are watching to see how much interest there is in it."
					},
				OwnerNotes = "Notes for Issue 1",
				Category = new BasicCategoryModel
				{
					CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
				},
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 2",
				Description = "A new test issue 2",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
				IssueStatus =
					new BasicStatusModel
					{
						StatusName = "Watching",
						StatusDescription =
							"The suggestion is interesting. We are watching to see how much interest there is in it."
					},
				OwnerNotes = "Notes for Issue 2",
				Category = new BasicCategoryModel
				{
					CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
				},
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
				IssueStatus =
					new BasicStatusModel
					{
						StatusName = "Watching",
						StatusDescription =
							"The suggestion is interesting. We are watching to see how much interest there is in it."
					},
				OwnerNotes = "Notes for Issue 3",
				Category = new BasicCategoryModel
				{
					CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
				},
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
			IssueStatus =
				new BasicStatusModel
				{
					StatusName = "Watching",
					StatusDescription =
						"The suggestion is interesting. We are watching to see how much interest there is in it."
				},
			OwnerNotes = "Notes for Issue 1",
			Category = new BasicCategoryModel
			{
				CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
			},
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
			IssueStatus =
				new BasicStatusModel
				{
					StatusName = "Watching",
					StatusDescription =
						"The suggestion is interesting. We are watching to see how much interest there is in it."
				},
			OwnerNotes = "Notes for Issue 1",
			Category = new BasicCategoryModel
			{
				CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
			},
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
			IssueStatus = new BasicStatusModel(),
			OwnerNotes = "Notes for Issue 1",
			Category = new BasicCategoryModel
			{
				CategoryName = "Miscellaneous", CategoryDescription = "Not sure where this fits."
			},
		};

		return issue;
	}

	public static IssueModel GetIssue(
		string id,
		string issueName,
		string description,
		DateTime dateCreated,
		bool archived,
		BasicStatusModel status,
		string ownerNotes,
		BasicCategoryModel category)
	{
		var issue = new IssueModel
		{
			Id = id,
			IssueName = issueName,
			Description = description,
			DateCreated = dateCreated,
			Archived = archived,
			IssueStatus = status,
			OwnerNotes = ownerNotes,
			Category = category
		};

		return issue;
	}
}