namespace IssueTracker.Library.Helpers.BogusFakes;

public static class FakeComment
{
	public static IEnumerable<CommentModel> GetComments(int numberOfComments)
	{

		var commentsGenerator = new Faker<CommentModel>()
			.RuleFor(x => x.Id, Guid.NewGuid().ToString)
			.RuleFor(c => c.Comment, f => f.Lorem.Sentence())
			.RuleFor(x => x.Issue, new BasicIssueModel(FakeIssue.GetIssues(1).First()))
			.RuleFor(c => c.Author, f => new BasicUserModel(FakeUser.GetUsers(1).First()))
			.RuleFor(c => c.DateCreated, f => f.Date.Past());

		return commentsGenerator.Generate(numberOfComments);

	}

	public static IEnumerable<BasicCommentModel> GetBasicComments(int numberOfComments)
	{

		var comments = GetComments(numberOfComments);
		var basicComments = comments.Select(c => new BasicCommentModel(c));

		return basicComments;

	}
}