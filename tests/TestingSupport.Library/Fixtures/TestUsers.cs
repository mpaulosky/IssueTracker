namespace TestingSupport.Library.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestUsers
{
	public static IEnumerable<UserModel> GetUsers()
	{
		List<UserModel> expected = new List<UserModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Jim",
				LastName = "Test",
				DisplayName = "jim test",
				EmailAddress = "jim.test@test.com",
				AuthoredIssues = new List<BasicIssueModel> { new(TestIssues.GetKnownIssue()) },
				AuthoredComments = new List<BasicCommentModel>
				{
					new() { Id = Guid.NewGuid().ToString(), Comment = Guid.NewGuid().ToString() }
				}
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Sam",
				LastName = "Test",
				DisplayName = "sam test",
				EmailAddress = "sam.test@test.com",
				AuthoredIssues =
					new List<BasicIssueModel>
					{
						new() { Id = Guid.NewGuid().ToString(), Issue = Guid.NewGuid().ToString() }
					},
				AuthoredComments = new List<BasicCommentModel>
				{
					new() { Id = Guid.NewGuid().ToString(), Comment = Guid.NewGuid().ToString() }
				}
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				ObjectIdentifier = Guid.NewGuid().ToString(),
				FirstName = "Tim",
				LastName = "Test",
				DisplayName = "tim test",
				EmailAddress = "tim.test@test.com",
				AuthoredIssues =
					new List<BasicIssueModel>
					{
						new() { Id = Guid.NewGuid().ToString(), Issue = Guid.NewGuid().ToString() }
					},
				AuthoredComments = new List<BasicCommentModel>
				{
					new() { Id = Guid.NewGuid().ToString(), Comment = Guid.NewGuid().ToString() }
				}
			}
		};

		return expected;
	}

	public static UserModel GetKnownUser()
	{
		UserModel user = new UserModel
		{
			Id = "5dc1039a1521eaa36835e545",
			ObjectIdentifier = "5dc1039a1521eaa36835e542",
			FirstName = "Jim",
			LastName = "Test",
			DisplayName = "jim test",
			EmailAddress = "jim.test@test.com",
			AuthoredIssues = new List<BasicIssueModel> { new(TestIssues.GetKnownIssue()) },
			AuthoredComments = new List<BasicCommentModel> { new(TestComments.GetKnownComment()) }
		};

		return user;
	}


	public static UserModel GetKnownUserWithNoVotedOn()
	{
		UserModel user = new UserModel
		{
			Id = "5dc1039a1521eaa36835e541",
			ObjectIdentifier = "5dc1039a1521eaa36835e542",
			FirstName = "Jim",
			LastName = "Test",
			DisplayName = "jim test",
			EmailAddress = "jim.test@test.com",
			AuthoredIssues = new List<BasicIssueModel> { new(TestIssues.GetKnownIssue()) },
			AuthoredComments = new List<BasicCommentModel> { new(TestComments.GetKnownComment()) }
		};

		return user;
	}

	public static UserModel GetUser(
		string userId,
		string objectIdentifier,
		string firstName,
		string lastName,
		string displayName,
		string email)
	{
		UserModel expected = new UserModel
		{
			Id = userId,
			ObjectIdentifier = objectIdentifier,
			FirstName = firstName,
			LastName = lastName,
			DisplayName = "jim test",
			EmailAddress = "jim.test@test.com",
			AuthoredIssues = new List<BasicIssueModel>(),
			AuthoredComments = new List<BasicCommentModel>()
		};

		return expected;
	}

	public static UserModel GetNewUser()
	{
		UserModel user = new UserModel
		{
			ObjectIdentifier = "5dc1039a1521eaa36835e542",
			FirstName = "Jim",
			LastName = "Test",
			DisplayName = "jim test",
			EmailAddress = "jim.test@test.com",
			AuthoredIssues = new List<BasicIssueModel> { new(TestIssues.GetKnownIssue()) },
			AuthoredComments = new List<BasicCommentModel> { new(TestComments.GetKnownComment()) }
		};

		return user;
	}

	public static UserModel GetUpdatedUser()
	{
		UserModel user = new UserModel
		{
			Id = "5dc1039a1521eaa36835e545",
			ObjectIdentifier = "5dc1039a1521eaa36835e542",
			FirstName = "Jim",
			LastName = "Test",
			DisplayName = "jim test Update",
			EmailAddress = "jim.test@test.com",
			AuthoredIssues = new List<BasicIssueModel> { new(TestIssues.GetKnownIssue()) },
			AuthoredComments = new List<BasicCommentModel> { new(TestComments.GetKnownComment()) }
		};

		return user;
	}
}