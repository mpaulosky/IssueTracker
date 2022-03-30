namespace IssueTracker.Library.UnitTests.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestIssues
{
	public static IEnumerable<Issue> GetIssues()
	{
		var issues = new List<Issue>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 1",
				Description = "A new test issue 1",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new Status(),
				OwnerNotes = "Notes for Issue 1",
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 2",
				Description = "A new test issue 2",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new Status(),
				OwnerNotes = "Notes for Issue 2",
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new Status(),
				OwnerNotes = "Notes for Issue 3",
			},
		};
		
		return issues;
	}

	public static IEnumerable<Issue> GetIssuesWithDuplicateAuthors()
	{
		var issues = new List<Issue>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 1",
				Description = "A new test issue 1",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = Guid.NewGuid().ToString(), DisplayName = "Tester" },
				IssueStatus = new Status(),
				OwnerNotes = "Notes for Issue 1",
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 2",
				Description = "A new test issue 2",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
				IssueStatus = new Status(),
				OwnerNotes = "Notes for Issue 2",
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				IssueName = "Test Issue 3",
				Description = "A new test issue 3",
				DateCreated = DateTime.UtcNow,
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
				IssueStatus = new Status(),
				OwnerNotes = "Notes for Issue 3",
			},
		};
		
		return issues;
	}
	
	public static Issue GetUpdatedIssue()
	{
		var issue = new Issue()
		{
			Id = "5dc1039a1521eaa36835e542",
			IssueName = "Updated Test Issue 1",
			Description = "Updated A new test issue 1",
			DateCreated = DateTime.UtcNow,
			Archived = false,
			Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
			IssueStatus = new Status(),
			OwnerNotes = "Notes for Issue 1",
		};

		return issue;
	}

	public static Issue GetKnownIssue()
	{
		var issue = new Issue()
		{
			Id = "5dc1039a1521eaa36835e542",
			IssueName = "Test Issue 1",
			Description = "A new test issue 1",
			DateCreated = DateTime.UtcNow,
			Archived = false,
			Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
			IssueStatus = new Status(),
			OwnerNotes = "Notes for Issue 1",
		};

		return issue;
	}

	public static Issue GetNewIssue()
	{
		var issue = new Issue()
		{
			IssueName = "Test Issue 1",
			Description = "A new test issue",
			DateCreated = DateTime.UtcNow,
			Archived = false,
			Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Tester" },
			IssueStatus = new Status(),
			OwnerNotes = "Notes for Issue 1",
		};

		return issue;
	}

	public static Issue GetIssue(
		string id, 
		string issueName, 
		string description, 
		DateTime dateCreated, 
		bool archived, 
		Status status, 
		string ownerNotes)
	{
		var issue = new Issue()
		{
			Id = id,
			IssueName = issueName,
			Description = description,
			DateCreated = dateCreated,
			Archived = archived,
			IssueStatus = status,
			OwnerNotes = ownerNotes,
		};

		return issue;
	}
}