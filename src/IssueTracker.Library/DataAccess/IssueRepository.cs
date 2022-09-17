//-----------------------------------------------------------------------
// <copyright file="IssueRepository.cs" company="mpaulosky">
//		Author:  Matthew Paulosky
//		Copyright (c) 2022. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

/// <summary>
///		IssueRepository class
/// </summary>
public class IssueRepository : IIssueRepository
{
	private readonly IMongoDbContextFactory _context;
	private readonly IMongoCollection<IssueModel> _issueCollection;
	private readonly IMongoCollection<UserModel> _userCollection;

	/// <summary>
	///		IssueRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	/// <exception cref="ArgumentNullException"></exception>
	public IssueRepository(IMongoDbContextFactory context)
	{
		_context = Guard.Against.Null(context, nameof(context));

		string issueCollectionName = GetCollectionName(nameof(IssueModel));

		_issueCollection = _context.GetCollection<IssueModel>(issueCollectionName);

		string userCollectionName = GetCollectionName(nameof(UserModel));

		_userCollection = _context.GetCollection<UserModel>(userCollectionName);
	}

	/// <summary>
	///		CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	/// <exception cref="Exception"></exception>
	public async Task CreateIssue(IssueModel issue)
	{
		using IClientSessionHandle session = await _context.Client.StartSessionAsync().ConfigureAwait(true);

		session.StartTransaction();

		try
		{
			IMongoCollection<IssueModel> issuesInTransaction = _issueCollection;

			await issuesInTransaction.InsertOneAsync(issue);

			IMongoCollection<UserModel> usersInTransaction = _userCollection;

			UserModel user = (await _userCollection.FindAsync(u => u.Id == issue.Author.Id)).First();

			user.AuthoredIssues.Add(new BasicIssueModel(issue));

			await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

			await session.CommitTransactionAsync();
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync();
			throw;
		}
	}

	/// <summary>
	///		GetIssue method
	/// </summary>
	/// <param name="issueId">string</param>
	/// <returns>Task of IssueModel</returns>
	public async Task<IssueModel> GetIssue(string issueId)
	{
		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", issueId);

		IssueModel result = (await _issueCollection.FindAsync(filter)).FirstOrDefault();

		return result;
	}

	/// <summary>
	///		GetIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetIssues()
	{
		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Empty;

		List<IssueModel> results = (await _issueCollection.FindAsync(filter)).ToList();

		return results;
	}

	/// <summary>
	///		GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetIssuesWaitingForApproval()
	{
		IEnumerable<IssueModel> output = await GetIssues();

		List<IssueModel> results = output.Where(x => x.ApprovedForRelease == false && x.Rejected == false).ToList();

		return results;
	}

	/// <summary>
	///		GetApprovedIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetApprovedIssues()
	{
		IEnumerable<IssueModel> output = await GetIssues();

		List<IssueModel> results = output.Where(x => x.ApprovedForRelease && x.Rejected == false).ToList();

		return results;
	}

	/// <summary>
	///		GetUserIssues method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetUsersIssues(string userId)
	{
		List<IssueModel> results = (await _issueCollection.FindAsync(s => s.Author.Id == userId)).ToList();

		return results;
	}

	/// <summary>
	///		UpdateIssue method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="issue">IssueModel</param>
	public async Task UpdateIssue(string id, IssueModel issue)
	{
		FilterDefinition<IssueModel> filter = Builders<IssueModel>.Filter.Eq("_id", id);

		await _issueCollection.ReplaceOneAsync(filter, issue);
	}
}