//-----------------------------------------------------------------------
// <copyright file="IssueRepository.cs" company="mpaulosky">
//     Author:  Matthew Paulosky
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace IssueTracker.Library.DataAccess;

/// <summary>
///   IssueRepository class
/// </summary>
public class IssueRepository : IIssueRepository
{
	private readonly IMongoCollection<IssueModel> _collection;
	private readonly IMongoDbContext _context;
	private readonly IMongoCollection<UserModel> _userCollection;

	/// <summary>
	///   IssueRepository constructor
	/// </summary>
	/// <param name="context">IMongoDbContext</param>
	public IssueRepository(IMongoDbContext context)
	{
		_context = context;
		_collection = context?.GetCollection<IssueModel>(GetCollectionName(nameof(IssueModel)));
		_userCollection = context?.GetCollection<UserModel>(GetCollectionName(nameof(UserModel)));
	}

	/// <summary>
	///   CreateIssue method
	/// </summary>
	/// <param name="issue">IssueModel</param>
	public async Task CreateIssue(IssueModel issue)
	{
		using var session = await _context.Client.StartSessionAsync().ConfigureAwait(true);

		session.StartTransaction();

		try
		{
			var issuesInTransaction = _collection;

			await issuesInTransaction.InsertOneAsync(issue).ConfigureAwait(true);

			var usersInTransaction = _userCollection;

			var user = (await _userCollection.FindAsync(u => u.Id == issue.Author.Id).ConfigureAwait(true)).First();

			user.AuthoredIssues.Add(new BasicIssueModel(issue));

			await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user).ConfigureAwait(true);

			await session.CommitTransactionAsync().ConfigureAwait(true);
		}
		catch (Exception)
		{
			await session.AbortTransactionAsync().ConfigureAwait(true);
			throw;
		}
	}

	/// <summary>
	///   GetIssue method
	/// </summary>
	/// <param name="id">string</param>
	/// <returns>Task of IssueModel</returns>
	public async Task<IssueModel> GetIssue(string id)
	{
		var objectId = new ObjectId(id);

		var filter = Builders<IssueModel>.Filter.Eq("_id", objectId);

		var result = await _collection.FindAsync(filter).ConfigureAwait(true);

		return result.FirstOrDefault();
	}

	/// <summary>
	///   GetIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetIssues()
	{
		var all = await _collection.FindAsync(Builders<IssueModel>.Filter.Empty).ConfigureAwait(true);

		return await all.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   GetIssuesWaitingForApproval method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetIssuesWaitingForApproval()
	{
		var output = await GetIssues().ConfigureAwait(true);
		return output.Where(x =>
			x.ApprovedForRelease == false
			&& x.Rejected == false).ToList();
	}

	/// <summary>
	///   GetApprovedIssues method
	/// </summary>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetApprovedIssues()
	{
		var output = await GetIssues().ConfigureAwait(true);
		return output.Where(x =>
			x.ApprovedForRelease
			&& x.Rejected == false).ToList();
	}

	/// <summary>
	///   GetUserIssues method
	/// </summary>
	/// <param name="userId">string</param>
	/// <returns>Task of IEnumerable IssueModel</returns>
	public async Task<IEnumerable<IssueModel>> GetUsersIssues(string userId)
	{
		var objectId = new ObjectId(userId);

		var results = await _collection.FindAsync(s => s.Author.Id == objectId.ToString()).ConfigureAwait(true);

		return await results.ToListAsync().ConfigureAwait(true);
	}

	/// <summary>
	///   UpdateIssue method
	/// </summary>
	/// <param name="id">string</param>
	/// <param name="issue">IssueModel</param>
	public async Task UpdateIssue(string id, IssueModel issue)
	{
		var objectId = new ObjectId(id);

		await _collection.ReplaceOneAsync(Builders<IssueModel>.Filter.Eq("_id", objectId), issue).ConfigureAwait(true);
	}
}