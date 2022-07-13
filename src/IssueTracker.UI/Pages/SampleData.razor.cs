namespace IssueTracker.UI.Pages;

public partial class SampleData
{
	private readonly HashSet<string> _votes = new() { "1", "2", "3" };

	private bool _commentsCreated;
	private UserModel _foundUser = new();
	private bool _statusesCreated;

	private async Task GenerateSampleData()
	{
		UserModel user = new()
		{
			FirstName = "Tim",
			LastName = "Corey",
			EmailAddress = "tim@test.com",
			DisplayName = "Sample Tim Corey",
			ObjectIdentifier = "abc-123"
		};
		await _userService.CreateUser(user);

		_foundUser = await _userService.GetUserFromAuthentication("abc-123");

		var statuses = await _statusService.GetStatuses();

		IssueModel issue = new()
		{
			Author = new BasicUserModel(_foundUser),
			IssueName = "Our First Issue",
			Description = "This is a suggestion created by the sample data generation method."
		};
		await _issueService.CreateIssue(issue);

		issue = new IssueModel
		{
			Author = new BasicUserModel(_foundUser),
			IssueName = "Our Second Issue",
			Description = "This is a issue created by the sample data generation method.",
			IssueStatus = statuses[0],
			OwnerNotes = "This is the note for the status."
		};
		await _issueService.CreateIssue(issue);

		issue = new IssueModel
		{
			Author = new BasicUserModel(_foundUser),
			IssueName = "Our Third Issue",
			Description = "This is a issue created by the sample data generation method.",
			IssueStatus = statuses[1],
			OwnerNotes = "This is the note for the status."
		};
		await _issueService.CreateIssue(issue);

		issue = new IssueModel
		{
			Author = new BasicUserModel(_foundUser),
			IssueName = "Our Forth Issue",
			Description = "This is a issue created by the sample data generation method.",
			IssueStatus = statuses[2],
			OwnerNotes = "This is the note for the status."
		};
		await _issueService.CreateIssue(issue);

		issue = new IssueModel
		{
			Author = new BasicUserModel(_foundUser),
			IssueName = "Our Fifth Issue",
			Description = "This is a issue created by the sample data generation method.",
			IssueStatus = statuses[3],
			OwnerNotes = "This is the note for the status."
		};
		await _issueService.CreateIssue(issue);
	}

	private async Task CreateStatuses()
	{
		var statuses = await _statusService.GetStatuses();
		if (statuses?.Count > 0)
		{
			return;
		}

		StatusModel stat = new()
		{
			StatusName = "Completed",
			StatusDescription = "The suggestion was accepted and the corresponding item was created."
		};
		await _statusService.CreateStatus(stat);

		stat = new StatusModel
		{
			StatusName = "Watching",
			StatusDescription = "The suggestion is interesting. We are watching to see how much interest there is in it."
		};
		await _statusService.CreateStatus(stat);

		stat = new StatusModel
		{
			StatusName = "Upcoming", StatusDescription = "The suggestion was accepted and it will be released soon."
		};
		await _statusService.CreateStatus(stat);

		stat = new StatusModel
		{
			StatusName = "Dismissed", StatusDescription = "The suggestion was not something that we are going to undertake."
		};
		await _statusService.CreateStatus(stat);

		_statusesCreated = true;
	}

	private async Task CreateComments()
	{
		var comments = await _commentService.GetComments();

		if (comments?.Count > 0)
		{
			return;
		}

		var comment = new CommentModel
		{
			Comment = "Test Comment 1",
			Archived = false,
			Author = new BasicUserModel(_foundUser),
			UserVotes = _votes,
			Status = new StatusModel()
		};
		await _commentService.CreateComment(comment);

		comment = new CommentModel
		{
			Comment = "Test Comment 2",
			Archived = false,
			Author = new BasicUserModel(_foundUser),
			UserVotes = _votes,
			Status = new StatusModel()
		};
		await _commentService.CreateComment(comment);

		comment = new CommentModel
		{
			Comment = "Test Comment 3",
			Archived = false,
			Author = new BasicUserModel(_foundUser),
			UserVotes = new HashSet<string>(),
			Status = new StatusModel()
		};
		await _commentService.CreateComment(comment);

		_commentsCreated = true;
	}
}