﻿//-----------------------------------------------------------------------// <copyright file="CommentComponent.razor.cs" company="mpaulosky">//		Author:  Matthew Paulosky//		Copyright (c) 2022. All rights reserved.// </copyright>//-----------------------------------------------------------------------namespace IssueTracker.UI.Components;public partial class CommentComponent{	private CommentModel? _archivingComment;	[Parameter] public CommentModel Item { get; set; } = new();	[Parameter] public UserModel LoggedInUser { get; set; } = new();

	/// <summary>
	///   VoteUp method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	public async Task VoteUp(CommentModel comment)
	{
		if (comment.Author.Id == LoggedInUser.Id)
		{
			return; // Can't vote on your own comments
		}

		if (!comment.UserVotes.Add(LoggedInUser.Id))
		{
			comment.UserVotes.Remove(LoggedInUser.Id);
		}

		await CommentService.UpVoteComment(comment.Id, LoggedInUser.Id);
	}

	/// <summary>
	///   GetUpVoteTopText method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string</returns>
	public string GetUpVoteTopText(CommentModel comment)
	{
		if (comment.UserVotes.Count > 0)
		{
			return comment.UserVotes.Count.ToString("00");
		}

		return comment.Author.Id == LoggedInUser.Id ? "Awaiting" : "Click To";
	}

	/// <summary>
	///   GetUpVoteBottomText method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string</returns>
	public string GetUpVoteBottomText(CommentModel comment)
	{
		return comment.UserVotes.Count > 1 ? "UpVotes" : "UpVote";
	}

	/// <summary>
	///   GetVoteCssClass method
	/// </summary>
	/// <param name="comment">CommentModel</param>
	/// <returns>string css class</returns>
	public string GetVoteCssClass(CommentModel comment)	{		if (comment.UserVotes.Count == 0)		{			return "comment-no-votes";		}		return comment.UserVotes.Contains(LoggedInUser.Id) ? "comment-not-voted" : "comment-voted";	}	private async Task ArchiveComment()	{		_archivingComment!.ArchivedBy = new BasicUserModel(LoggedInUser);		_archivingComment!.Archived = true;		await CommentService.UpdateComment(_archivingComment);		_archivingComment = null;	}}
