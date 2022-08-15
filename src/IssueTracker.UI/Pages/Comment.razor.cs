namespace IssueTracker.UI.Pages;

/// <summary>
/// Comment page class.
/// </summary>
public partial class Comment
{
	[Parameter]
	public string Id { get; set; }

	private CreateCommentModel _comment = new();

	private UserModel _loggedInUser;

	private IssueModel _issue;

	/// <summary>
	/// OnInitializedAsync event.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{

		_issue = await IssueService.GetIssue(Id);
		_loggedInUser = await AuthProvider.GetUserFromAuth(UserService);

	}

	/// <summary>
	/// CreateComment method.
	/// </summary>
	private async Task CreateComment()
	{

		CommentModel comment = new()
		{
			Issue = new BasicIssueModel(_issue),
			Author = new BasicUserModel(_loggedInUser),
			Comment = _comment.Comment,
		};

		await CommentService.CreateComment(comment);

		_comment = new CreateCommentModel();
		ClosePage();

	}

	/// <summary>
	/// ClosePage method.
	/// </summary>
	private void ClosePage()
	{

		NavManager.NavigateTo("/");

	}
	
}