namespace TestingSupport.Library.Fixtures;

[ExcludeFromCodeCoverage]
public static class TestComments
{
	public static CommentModel GetKnownComment()
	{
		CommentModel comment = new CommentModel
		{
			Id = "5dc1039a1521eaa36835e541",
			Comment = "Test Comment",
			Archived = false,
			Author = new BasicUserModel("5dc1039a1521eaa36835e541", "Test User"),
			DateCreated = DateTime.UtcNow,
			UserVotes = new HashSet<string> { "5dc1039a1521eaa36835e545" }
		};

		return comment;
	}

	public static CommentModel GetUpdatedComment()
	{
		CommentModel comment = new CommentModel
		{
			Id = "5dc1039a1521eaa36835e541",
			Comment = "Update Test Comment",
			Archived = false,
			Author = new BasicUserModel("5dc1039a1521eaa36835e541", "Test User"),
			DateCreated = DateTime.UtcNow,
			UserVotes = new HashSet<string> { "5dc1039a1521eaa36835e545" }
		};

		return comment;
	}

	public static IEnumerable<CommentModel> GetComments()
	{
		List<CommentModel> comments = new List<CommentModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				Comment = "Test Comment 1",
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e543", DisplayName = "Test User" },
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string>()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				Comment = "Test Comment 2",
				Archived = false,
				Author = new BasicUserModel(TestUsers.GetKnownUser()),
				UserVotes = new HashSet<string>()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				Comment = "Test Comment 3",
				Archived = false,
				Author = new BasicUserModel("5dc1039a1521eaa36835e543", "Test User"),
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string> { "5dc1039a1521eaa36835e545", "5dc1039a1521eaa36835e544" }
			}
		};

		return comments;
	}

	public static IEnumerable<CommentModel> GetCommentsWithDuplicateAuthors()
	{
		List<CommentModel> comments = new List<CommentModel>
		{
			new()
			{
				Id = Guid.NewGuid().ToString(),
				Comment = "Test Comment 1",
				Archived = false,
				Author = new BasicUserModel { Id = "5dc1039a1521eaa36835e541", DisplayName = "Test User" },
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string>()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				Comment = "Test Comment 2",
				Archived = false,
				Author = new BasicUserModel(TestUsers.GetKnownUser()),
				UserVotes = new HashSet<string>()
			},
			new()
			{
				Id = Guid.NewGuid().ToString(),
				Comment = "Test Comment 3",
				Archived = false,
				Author = new BasicUserModel("5dc1039a1521eaa36835e541", "Test User"),
				DateCreated = DateTime.UtcNow,
				UserVotes = new HashSet<string>()
			}
		};

		return comments;
	}

	public static CommentModel GetComment(string id, string commentName, bool archived)
	{
		CommentModel comment = new CommentModel { Id = id, Comment = commentName, Archived = archived };

		return comment;
	}

	public static CommentModel GetNewComment()
	{
		CommentModel comment = new CommentModel
		{
			Comment = "Test Comment",
			Archived = false,
			Author = new BasicUserModel("5dc1039a1521eaa36835e541", "Test User"),
			DateCreated = DateTime.UtcNow
		};

		return comment;
	}
}