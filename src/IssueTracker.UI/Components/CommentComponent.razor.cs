//-----------------------------------------------------------------------
// <copyright file="CommentComponent.razor.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.UI.Components;

public partial class CommentComponent
{
	[Parameter] public CommentModel Item { get; set; }

	[Parameter] public UserModel LoggedInUser { get; set; } = new();

	/// <summary>
	///		VoteUp method
	/// </summary>
	/// <param name = "comment">CommentModel</param>
	private async Task VoteUp(CommentModel comment)
	{
		if (LoggedInUser is not null)
		{
			if (comment.Author.Id == LoggedInUser.Id) return; // Can't vote on your own comments

			if (comment.UserVotes.Add(LoggedInUser.Id) == false) comment.UserVotes.Remove(LoggedInUser.Id);

			await CommentService.UpVoteComment(comment.Id, LoggedInUser.Id);
		}
	}

	/// <summary>
	///		GetUpVoteTopText method
	/// </summary>
	/// <param name = "comment">CommentModel</param>
	/// <returns>string</returns>
	private string GetUpVoteTopText(CommentModel comment)
	{
		if (comment.UserVotes?.Count > 0) return comment.UserVotes.Count.ToString("00");

		return comment.Author.Id == LoggedInUser?.Id ? "Awaiting" : "Click To";
	}

	/// <summary>
	///		GetUpVoteBottomText method
	/// </summary>
	/// <param name = "comment">CommentModel</param>
	/// <returns>string</returns>
	private static string GetUpVoteBottomText(CommentModel comment)
	{
		return comment.UserVotes?.Count > 1 ? "UpVotes" : "UpVote";
	}

	/// <summary>
	///		GetVoteCssClass method
	/// </summary>
	/// <param name = "comment">CommentModel</param>
	/// <returns>string css class</returns>
	private string GetVoteCssClass(CommentModel comment)
	{
		if (comment.UserVotes is null || comment.UserVotes.Count == 0) return "issue-detail-no-votes";

		return comment.UserVotes.Contains(LoggedInUser?.Id) ? "issue-detail-voted" : "issue-detail-not-voted";
	}
}