﻿//-----------------------------------------------------------------------
// <copyright file="IssueRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace IssueTracker.PlugIns.DataAccess;

/// <summary>
///		IssueRepository class
/// </summary>
public class IssueRepository : IIssueRepository
{

	private readonly IMongoCollection<IssueModel> _issueCollection;

	/// <summary>
	///		IssueRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public IssueRepository(IMongoDbContextFactory context)
	{

		Guard.Against.Null(context, nameof(context));

		var issueCollectionName = GetCollectionName(nameof(IssueModel));

		_issueCollection = context.GetCollection<IssueModel>(issueCollectionName);

	}

	/// <summary>
	/// Archive Issue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <returns>Task</returns>
	public async Task ArchiveAsync(IssueModel issue)
	{

		var objectId = new ObjectId(issue.Id);

		// Archive the issue
		issue.Archived = true;

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		await _issueCollection.ReplaceOneAsync(filter, issue);

	}

	/// <summary>
	///		CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateAsync(IssueModel issue)
	{

		await _issueCollection.InsertOneAsync(issue);

	}

	///  <summary>
	/// 		GetIssue method
	///  </summary>
	///  <param name="itemId">string</param>
	///  <returns>Task of IssueModel</returns>
	public async Task<IssueModel> GetAsync(string itemId)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		IssueModel result = (await _issueCollection.FindAsync(filter)).FirstOrDefault();

		return result;

	}

	/// <summary>
	///		GetIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetAllAsync()
	{

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Empty;

		var results = (await _issueCollection.FindAsync(filter)).ToList();

		return results;

	}

	/// <summary>
	///		GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetWaitingForApprovalAsync()
	{

		IEnumerable<IssueModel> output = await GetAllAsync();

		var results = output.Where(x => !(x is { ApprovedForRelease: true }) && !x.Rejected).ToList();

		return results;

	}

	/// <summary>
	///		GetApprovedIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetApprovedAsync()
	{

		IEnumerable<IssueModel> output = await GetAllAsync();

		var results = output.Where(x => x.ApprovedForRelease && !x.Rejected).ToList();

		return results;

	}

	/// <summary>
	///		GetUserIssues method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetByUserAsync(string userId)
	{

		var results = (await _issueCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;

	}

	/// <summary>
	///		UpdateIssue method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="issue">IssueModel</param>
	public async Task UpdateAsync(string itemId, IssueModel issue)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		await _issueCollection.ReplaceOneAsync(filter, issue);

	}

}
