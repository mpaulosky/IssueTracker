//-----------------------------------------------------------------------
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
	///		CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateIssueAsync(IssueModel issue)
	{

		await _issueCollection.InsertOneAsync(issue);

	}

	///  <summary>
	/// 		GetIssue method
	///  </summary>
	///  <param name="itemId">string</param>
	///  <returns>Task of IssueModel</returns>
	public async Task<IssueModel> GetIssueAsync(string itemId)
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
	public async Task<IEnumerable<IssueModel>> GetIssuesAsync()
	{

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Empty;

		var results = (await _issueCollection.FindAsync(filter)).ToList();

		return results;

	}

	/// <summary>
	///		GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetIssuesWaitingForApprovalAsync()
	{

		IEnumerable<IssueModel> output = await GetIssuesAsync();

		var results = output.Where(x => !x.ApprovedForRelease && !x.Rejected).ToList();

		return results;

	}

	/// <summary>
	///		GetApprovedIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetApprovedIssuesAsync()
	{

		IEnumerable<IssueModel> output = await GetIssuesAsync();

		var results = output.Where(x => x.ApprovedForRelease && !x.Rejected).ToList();

		return results;

	}

	/// <summary>
	///		GetUserIssues method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetIssuesByUserAsync(string userId)
	{

		var results = (await _issueCollection.FindAsync(s => s.Author!.Id == userId)).ToList();

		return results;

	}

	/// <summary>
	///		UpdateIssue method
	/// </summary>
	/// <param name="itemId">string</param>
	/// <param name="issue">IssueModel</param>
	public async Task UpdateIssueAsync(string itemId, IssueModel issue)
	{

		var objectId = new ObjectId(itemId);

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		await _issueCollection.ReplaceOneAsync(filter, issue);

	}

	public async Task ArchiveIssueAsync(IssueModel issue)
	{

		var objectId = new ObjectId(issue.Id);

		// Archive the issue
		issue.Archived = true;

		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		await _issueCollection.ReplaceOneAsync(filter, issue);

	}

}
